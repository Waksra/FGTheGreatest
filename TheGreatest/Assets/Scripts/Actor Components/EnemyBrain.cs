using System;
using System.Collections;
using System.Resources;
using UnityEngine;

namespace Actor_Components
{
    public class EnemyBrain : MonoBehaviour
    {
        public float idleDistance = 4.2f;
        public Vector2 sweepPlayerDistances;
        
        public float evadeCooldown;
        public float sweepCooldown;
        public float shootCooldown;
        
        public float circleCastRadius;
        public float circleCastDistance;
        public string projectileTag;
        public LayerMask circleCastLayerMask;
        
        [Space(10f)] public bool debug;
        
        private const float SeekFrequency = 0.05f;
        private const float DecideFrequency = 0.1f;
        private const float ActFrequency = 0.2f;

        private float _evadeTimer = 0f;
        private float _sweepTimer = 0f;
        private float _shootTimer = 0f;

        private Actions _decidedAction = Actions.Idle;
        private Actions _currentAction = Actions.Idle;

        private Vector2 _playerPosition;
        private float _projectileDistance;

        private bool _isManeuver;
        private bool _canShoot = true;

        private RaycastHit2D _seekHit;

        private Transform _transform;
        private MovementController _movementController;
        private ShootingController _shootingController;

        private enum Actions
        {
            Idle,
            MoveToIdle,
            Sweep,
            Track,
            Evade,
            Flee
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _movementController = GetComponent<MovementController>();
            _shootingController = GetComponent<ShootingController>();
        }

        private void Start()
        {
            StartCoroutine(Seek());
            StartCoroutine(Decide());
            StartCoroutine(Act());
        }

        private IEnumerator Seek()
        {
            yield return new WaitForSeconds(SeekFrequency);
            
            while (true)
            {
                _playerPosition = PlayerBrain.PlayerPosition;
                
                _seekHit = Physics2D.CircleCast(transform.position, circleCastRadius, Vector2.down, 
                    circleCastDistance, circleCastLayerMask);

                if (_seekHit && _seekHit.collider.CompareTag(projectileTag))
                {
                    _projectileDistance = _seekHit.distance;
                }
                else
                {
                    _projectileDistance = -1;
                }

                if (!_canShoot)
                {
                    _shootTimer -= SeekFrequency;
                    if (_shootTimer <= 0)
                    {
                        _canShoot = true;
                        _shootTimer = shootCooldown;
                    }
                }
                
                yield return new WaitForSeconds(SeekFrequency);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator Decide()
        {
            yield return new WaitForSeconds(DecideFrequency);
            
            while (true)
            {
                if (_projectileDistance > -1 && _evadeTimer <= 0)
                {
                    _decidedAction = Actions.Evade;
                }
                else if (_transform.position.y >= idleDistance && _sweepTimer <= 0)
                {
                    _decidedAction = Actions.Sweep;
                }
                else if (_transform.position.y < idleDistance)
                {
                    _decidedAction = Actions.MoveToIdle;
                }
                else
                {
                    _decidedAction = Actions.Idle;
                }
                

                if(_evadeTimer > 0)
                    _evadeTimer -= DecideFrequency;
                if (_sweepTimer > 0)
                    _sweepTimer -= DecideFrequency;
                
                yield return new WaitForSeconds(DecideFrequency);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator Act()
        {
            yield return new WaitForSeconds(ActFrequency);
            
            while (true)
            {
                if(_decidedAction != _currentAction && !_isManeuver)
                    switch (_decidedAction)
                    {
                        case Actions.Idle:
                            _currentAction = Actions.Idle;
                            _movementController.MoveVector = Vector2.zero;
                            break;
                        case Actions.MoveToIdle:
                            _currentAction = Actions.MoveToIdle;
                            StartCoroutine(MoveToIdleManeuver());
                            break;
                        case Actions.Sweep:
                            _currentAction = Actions.Sweep;
                            int direction = (int) Mathf.Sign(_playerPosition.x - _transform.position.x);
                            StartCoroutine(SweepManeuver(direction));
                            break;
                        case Actions.Track:
                            break;
                        case Actions.Evade:
                            break;
                        case Actions.Flee:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                yield return new WaitForSeconds(ActFrequency);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator MoveToIdleManeuver()
        {
            _isManeuver = true;
            _movementController.MoveVector = Vector2.up;
            while (_transform.position.y < idleDistance)
            {
                TryFireProjectile();
                yield return null;
            }
            _movementController.MoveVector = Vector2.zero;
            _transform.position.Set(_transform.position.x, idleDistance, _transform.position.z);

            _isManeuver = false;
        }

        private IEnumerator SweepManeuver(int direction)
        {
            _isManeuver = true;
            float initialXPosition = _transform.position.x;

            while (_transform.position.y > _playerPosition.y && 
                   Mathf.Abs(initialXPosition - _transform.position.x) < 
                   Mathf.Abs(initialXPosition - (_playerPosition.x + sweepPlayerDistances.x * direction)))
            {
                _movementController.MoveVector = Vector3.Slerp(Vector2.down,
                    direction > 0 ? Vector2.right : Vector2.left,
                    1 - ((_transform.position.y - (_playerPosition.y + sweepPlayerDistances.y)) / 
                        (idleDistance - (_playerPosition.y + sweepPlayerDistances.y))));
                
                TryFireProjectile();
                
                yield return null;
            }

            _sweepTimer = sweepCooldown;
            _isManeuver = false;
        }

        private void TryFireProjectile()
        {
            if (!_canShoot)
                return;
            
            _shootingController.Shoot();
            _canShoot = false;
        }

        private void OnEnable()
        {
            Reset();
            if (_transform.position.y > idleDistance)
            {
                _movementController.lockInBoundary = false;
                StartCoroutine(MoveFromSpawn());
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Reset()
        {
            _decidedAction = Actions.Idle;
            _currentAction = Actions.Idle;
            _isManeuver = false;

            _evadeTimer = 0;
            _sweepTimer = 0;
            _shootTimer = 0;

            StartCoroutine(Seek());
            StartCoroutine(Decide());
            StartCoroutine(Act());
        }

        private IEnumerator MoveFromSpawn()
        {
            _isManeuver = true;
            _movementController.MoveVector = Vector2.down;
            while (_transform.position.y > idleDistance)
            {
                yield return null;
            }
            _movementController.MoveVector = Vector2.zero;
            _transform.position.Set(_transform.position.x, idleDistance, _transform.position.z);

            _movementController.lockInBoundary = true;
            _isManeuver = false;
        }

        private void OnDrawGizmos()
        {
            if(!debug)
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _seekHit.point);
            Gizmos.DrawWireSphere(_seekHit.point, circleCastRadius);
        }
    }
}
