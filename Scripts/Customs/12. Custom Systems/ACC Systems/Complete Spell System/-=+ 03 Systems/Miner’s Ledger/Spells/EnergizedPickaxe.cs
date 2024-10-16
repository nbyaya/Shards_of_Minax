using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class EnergizedPickaxe : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Energized Pickaxe", "Boost Miners!",
            //SpellCircle.Fourth, // Adjust if needed
            21002,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public EnergizedPickaxe(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Caster.SendMessage("You focus your energy into your pickaxe, enhancing your mining capabilities!");

				// Play sound and visual effects
				Caster.PlaySound(0x5C9); // Sound effect for magic enhancement
				Effects.SendLocationParticles(
					Caster,       // IEntity (using Caster directly)
					0x3728,      // Effect ID
					10,          // Speed
					15,          // Duration
					1153,        // Hue (color)
					0,           // Render mode
					0,           // Layer
					0            // Unknown parameter (usually 0)
				);

				Caster.BeginTarget(1, false, TargetFlags.None, new TargetCallback(OnTarget));
			}
			FinishSequence();
		}


        public void OnTarget(Mobile from, object obj)
        {
            if (obj is Pickaxe || obj is Shovel)
            {
                Item tool = (Item)obj;

                // Enhance tool for a limited time
                tool.Hue = 1153; // Change color to show enhancement
                Caster.SendMessage("Your tool is now energized!");

                // Increase mining speed and yield
                Caster.Delta(MobileDelta.Hue);
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => EndEffect(tool));
            }
            else
            {
                from.SendMessage("You can only use this spell on a mining tool.");
            }
        }

        private void EndEffect(Item tool)
        {
            if (tool != null && !tool.Deleted)
            {
                tool.Hue = 0; // Revert color back to normal
                tool.InvalidateProperties();
                if (tool.RootParent is Mobile owner)
                    owner.SendMessage("The magical energy fades from your tool.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
