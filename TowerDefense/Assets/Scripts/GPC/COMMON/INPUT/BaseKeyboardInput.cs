using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Harris.GPC
{

	[AddComponentMenu("CSharpBookCode/Common/Keyboard Input Controller")]

	public abstract class BaseKeyboardInput : BaseInputController
	{

		//Reference to player input actions
		public PlayerInputActions InputActions {get; set;}

		//input actions
		protected InputAction wasdAction;
		protected InputAction spaceBarAction;
		protected InputAction shitfBtnAction;
		protected InputAction lShiftBtnAction;

		public virtual void Initialize()
		{
			wasdAction = InputActions.Player.WASD;
			lShiftBtnAction = InputActions.Player.Shift;
		}

		public void LateUpdate()
		{
			// check inputs each LateUpdate() ready for the next tick
			CheckInput();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(false);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}