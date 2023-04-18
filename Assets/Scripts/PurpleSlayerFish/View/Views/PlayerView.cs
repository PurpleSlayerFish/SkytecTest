using PurpleSlayerFish.Core.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PurpleSlayerFish.View.Views
{
    public class PlayerView : AbstractView<PlayerView>
    {
        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput PlayerInput => _playerInput;
        [SerializeField] private GameObject[] _trails;
        public GameObject[] Trails => _trails;
        [SerializeField] private GameObject _laserFlash;
        public GameObject LaserFlash => _laserFlash;
        [SerializeField] private GameObject _laser;
        public GameObject Laser => _laser;
        
    }
}