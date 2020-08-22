using System;
using System.Collections;
using UnityEngine;

namespace Actor_Components
{
    public class EnemyBrain : MonoBehaviour
    {
        public float idleDistance = 4.2f;
        public float sweepDistance = 2f;
        
        public float evadeCooldown;
        public float sweepCooldown;
        
        public float circleCastRadius;
        public float circleCastDistance;
        public string projectileTag;
        public LayerMask circleCastLayerMask;
        
        [Space(10f)] public bool debug;
        
        private const float SeekFrequency = 0.1f;
        private const float DecideFrequency = 0.3f;
        private const float ActFrequency = 0.6f;

        private float evadeTimer = 0f;
        private float sweepTimer = 0f;

        private Actions _decidedAction = Actions.Idle;
        private Actions _currentAction = Actions.Idle;

        private Vector2 _playerPosition;
        private float _projectileDistance;

        private bool isManeuver = false;

        private RaycastHit2D _seekHit;

        private Transform _transform;
        private MovementController _movementController;

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
                
                yield return new WaitForSeconds(SeekFrequency);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator Decide()
        {
            yield return new WaitForSeconds(DecideFrequency);
            
            while (true)
            {
                if (_projectileDistance > -1 && evadeTimer <= 0)
                {
                    _decidedAction = Actions.Evade;
                }
                else if (_transform.position.y >= idleDistance && sweepTimer <= 0)
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
                

                if(evadeTimer > 0)
                    evadeTimer -= DecideFrequency;
                if (sweepTimer > 0)
                    sweepTimer -= DecideFrequency;
                
                yield return new WaitForSeconds(DecideFrequency);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator Act()
        {
            yield return new WaitForSeconds(ActFrequency);
            
            while (true)
            {
                if(_decidedAction != _currentAction && !isManeuver)
                    switch (_decidedAction)
                    {
                        case Actions.Idle:
                            _currentAction = Actions.Idle;
                            _movementController.MoveVector = Vector2.zero;
                            Debug.Log("Idle.");
                            break;
                        case Actions.MoveToIdle:
                            _currentAction = Actions.MoveToIdle;
                            _movementController.MoveVector = Vector2.up;
                            Debug.Log("Move to Idle.");
                            break;
                        case Actions.Sweep:
                            _currentAction = Actions.Sweep;
                            int direction = (int) Mathf.Sign(_playerPosition.x - _transform.position.x);
                            StartCoroutine(SweepManeuver(direction));
                            Debug.Log("Sweep.");
                            break;
                        case Actions.Track:
                            break;
                        case Actions.Evade:
                            Debug.Log("Evade.");
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

        private IEnumerator SweepManeuver(int direction)
        {
            isManeuver = true;

            float distanceToSweep = idleDistance - sweepDistance;

            while (_transform.position.y > sweepDistance + 0.5f || direction * (_transform.position.x + _playerPosition.x) <= 0)
            {
                _movementController.MoveVector = Vector2.Lerp(Vector2.down,
                    direction > 0 ? Vector2.right : Vector2.left,
                    1 - ((_transform.position.y - sweepDistance) / distanceToSweep));
                
                yield return null;
            }

            sweepTimer = sweepCooldown;
            isManeuver = false;
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
