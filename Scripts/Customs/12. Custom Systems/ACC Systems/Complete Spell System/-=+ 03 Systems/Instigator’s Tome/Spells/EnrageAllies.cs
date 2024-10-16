using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class EnrageAllies : ProvocationSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Enrage Allies", "Rage En Flamma",
            21005, // Icon ID
            9500 // Cast sound ID
        );

        public override SpellCircle Circle => SpellCircle.First;
        public override double CastDelay => 1.5;
        public override double RequiredSkill => 70.0;
        public override int RequiredMana => 30;

        public EnrageAllies(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x208); // Play casting sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052); // Red sparkles

                List<Mobile> allies = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature bc && m.Alive && !m.IsDeadBondedPet && IsFriend(m, Caster) && m != Caster)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    ally.FixedParticles(0x3779, 10, 15, 5012, EffectLayer.Waist); // Flame effect on allies
                    ally.PlaySound(0x208); // Enrage sound

                    BaseCreature bc = ally as BaseCreature;
                    if (bc != null)
                    {
                        IncreaseAggression(bc);

                        bc.DamageMin += 5; // Increase minimum damage
                        bc.DamageMax += 5; // Increase maximum damage

                        // Adjust resistances using custom logic or modifiers
                        AdjustResistances(bc, -10);

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => RestoreDefense(bc)); // Restore defense after 30 seconds
                    }
                }
            }

            FinishSequence();
        }

		private void RestoreDefense(BaseCreature bc)
		{
			if (bc == null || bc.Deleted)
				return;

			DecreaseAggression(bc);

			bc.DamageMin -= 5; // Reset damage increase
			bc.DamageMax -= 5; // Reset damage increase

			AdjustResistances(bc, 0); // Restore resistances by removing modifiers
		}


        private void IncreaseAggression(BaseCreature bc)
        {
            // Implement your custom logic to increase aggression
        }

        private void DecreaseAggression(BaseCreature bc)
        {
            // Implement your custom logic to decrease aggression
        }

		private void AdjustResistances(BaseCreature bc, int amount)
		{
			if (amount != 0)
			{
				bc.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, amount));
				bc.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, amount));
				bc.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, amount));
				bc.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, amount));
				bc.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, amount));
			}
			else
			{
				// Reset resistances to default by removing modifiers
				bc.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 0));
				bc.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, 0));
				bc.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, 0));
				bc.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, 0));
				bc.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, 0));
			}
		}


        private bool IsFriend(Mobile mobile, Mobile caster)
        {
            // Implement custom friendship logic here
            return caster.CanBeHarmful(mobile); // Replace with your actual logic if needed
        }

        private class InternalTarget : Target
        {
            private EnrageAllies m_Owner;

            public InternalTarget(EnrageAllies owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
