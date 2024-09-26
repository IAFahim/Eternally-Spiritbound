using Sisus.Init;
using UnityEngine;
using Pancake.StatModifier;

namespace _Root.Scripts.Game.Init_Tests
{
	/// <summary>
	/// Initializer for the <see cref="PlayerController"/> component.
	/// </summary>
	internal sealed class PlayerControllerInitializer : Initializer<PlayerController, Rigidbody, Camera, IStatModifierFactory>
	{
		#if UNITY_EDITOR
		/// <summary>
		/// This section can be used to customize how the Init arguments will be drawn in the Inspector.
		/// <para>
		/// The Init argument names shown in the Inspector will match the names of members defined inside this section.
		/// </para>
		/// <para>
		/// Any PropertyAttributes attached to these members will also affect the Init arguments in the Inspector.
		/// </para>
		/// </summary>
		private sealed class Init
		{
			public Rigidbody Rb = default;
			public Camera Cam = default;
			public IStatModifierFactory StatModifierFactory = default;
		}
		#endif
	}
}
