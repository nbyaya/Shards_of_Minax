using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class BattlefieldAwareness : TacticsSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Battlefield Awareness", "In Wis Xen",
            //SpellCircle.Second,
            21004,
            9300,
            false,
            Reagent.Nightshade
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 20;

        public BattlefieldAwareness(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private readonly BattlefieldAwareness m_Owner;

            public InternalTarget(BattlefieldAwareness owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p.ToPoint3D()))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p.ToPoint3D());
                SpellHelper.GetSurfaceTop(ref p);

                // Play detection sound and visual effect
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE); // Sound effect for awareness
                Effects.SendLocationParticles(EffectItem.Create(p.ToPoint3D(), Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Visual effect for detection

                List<Mobile> detectedEnemies = new List<Mobile>();
                List<Item> detectedTraps = new List<Item>();

                // Scan for hidden mobiles and traps within a radius of 8 tiles
                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(p.ToPoint3D(), 8);

                foreach (Mobile m in eable)
                {
                    if (m.Hidden && m.AccessLevel == AccessLevel.Player) // Only detect players who are hidden
                    {
                        detectedEnemies.Add(m);
                    }
                }

                eable.Free();

                IPooledEnumerable itemEnum = Caster.Map.GetItemsInRange(p.ToPoint3D(), 8);

                foreach (Item item in itemEnum)
                {
                    if (item is TrapableContainer || item is BaseTrap) // Detect trap items
                    {
                        detectedTraps.Add(item);
                    }
                }

                itemEnum.Free();

                // Reveal detected enemies and traps
                foreach (Mobile m in detectedEnemies)
                {
                    m.RevealingAction();
                    m.FixedParticles(0x375A, 9, 20, 5044, EffectLayer.Head);
                    m.PlaySound(0x56D); // Sound effect when an enemy is revealed
                    Caster.SendMessage($"You have detected a hidden enemy: {m.Name}.");
                }

                foreach (Item trap in detectedTraps)
                {
                    trap.PublicOverheadMessage(MessageType.Regular, 0, false, "A trap is detected here!");
                    Caster.SendMessage("You have detected a trap!");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }

    // Extension method to convert IPoint3D to Point3D
    public static class Extensions
    {
        public static Point3D ToPoint3D(this IPoint3D p)
        {
            return new Point3D(p.X, p.Y, p.Z);
        }
    }
}
