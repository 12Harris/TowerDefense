using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Harris.GPC;

namespace TowerDefense
{

	[AddComponentMenu("CSharpBookCode/Common/Mouse Game Input Controller")]

	public class MouseInput : BaseMouseInput
	{

   	 	private Command leftMouseClickCmd;
		public Command LeftMouseClickCmd => leftMouseClickCmd;

		private void ConnectReceiver(string cmdType, ICommandReceiver receiver)
		{
			if(cmdType == "LeftMouseClickCmd")
			    leftMouseClickCmd.SetReceiver(receiver);
		}

		public override void Initialize()
		{
			base.Initialize();

			//Inititalize Commands
            leftMouseClickCmd = new Command(LeftMouseClickAction, "LeftMouseClickCmd");

			LeftMouseClickAction.Enable();

			ICommandReceiver._onBindReceiver += ConnectReceiver;
		}

		private void OnDisable()
		{
            leftMouseClickCmd.Action.Disable();
		}

		public void Update()
		{
            
		}
	}
}