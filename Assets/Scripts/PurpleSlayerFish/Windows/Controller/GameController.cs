using System;
using PurpleSlayerFish.Core.Windows;
using PurpleSlayerFish.Model.Services;
using PurpleSlayerFish.Windows.Window;
using TMPro;
using UnityEngine;

namespace PurpleSlayerFish.Windows.Controller
{
    public class GameController : AbstractController<GameWindow>
    {
        public const string UPDATE_UI_SCORE = "update_ui_score";
        public const string UPDATE_UI_POSITION = "update_ui_position";
        public const string UPDATE_UI_ROTATION = "update_ui_rotation";
        public const string UPDATE_UI_VELOCITY = "update_ui_velocity";
        public const string UPDATE_UI_LASER_COUNT = "update_ui_laser_count";
        public const string UPDATE_UI_LASER_RESTORATION_TIME = "update_ui_laser_restoration_time";

        private StringUtils _stringUtils = new();
        private MathUtils _mathUtils = new();

        protected override void AfterInitialize()
        {
            _subscriptionObserver.Subscribe(UPDATE_UI_SCORE, UpdateScore);
            _subscriptionObserver.Subscribe(UPDATE_UI_POSITION, UpdatePosition);
            _subscriptionObserver.Subscribe(UPDATE_UI_ROTATION, (Action<float>) UpdateRotation);
            _subscriptionObserver.Subscribe(UPDATE_UI_VELOCITY, (Action<float>) UpdateVelocity);
            _subscriptionObserver.Subscribe(UPDATE_UI_LASER_COUNT, (Action<int>) UpdateLaserCount);
            _subscriptionObserver.Subscribe(UPDATE_UI_LASER_RESTORATION_TIME, (Action<float>) UpdateLaserCooldown);
        }

        private void UpdatePosition(Vector2 value) =>
            UpdateText(_window.Position, _stringUtils.FromVector2(value, "\n", 100));

        private void UpdateScore(int value) => UpdateText(_window.Score, value.ToString());
        private void UpdateRotation(float value) => UpdateText(_window.Rotation, _stringUtils.FromFloat(_mathUtils.NormalizeAngle(value), 10));
        private void UpdateVelocity(float value) => UpdateText(_window.Velocity, _stringUtils.FromFloat(value, 10));
        private void UpdateLaserCount(int value) => UpdateText(_window.LaserCount, value.ToString());
        private void UpdateLaserCooldown(float value) => UpdateText(_window.LaserRestoration, _stringUtils.MinutesAndSeconds(Math.Max(0, value)));
        private void UpdateText(TMP_Text tmpText, string value) => tmpText.text = value;
    }
}