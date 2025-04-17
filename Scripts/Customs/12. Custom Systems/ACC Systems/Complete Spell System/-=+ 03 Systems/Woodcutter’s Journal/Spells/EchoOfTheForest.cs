using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class EchoOfTheForest : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Echo of the Forest", "Tal Silarna",
            21005, // Icon ID
            9305   // Cast sound ID
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 18; } }

        public EchoOfTheForest(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private EchoOfTheForest m_Owner;

            public InternalTarget(EchoOfTheForest owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                {
                    m_Owner.SummonSpirit(from, p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void SummonSpirit(Mobile caster, IPoint3D p)
        {
            Map map = caster.Map;

            if (map == null || !caster.InRange(p, 12) || !caster.CanSee(p))
            {
                caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            SpellHelper.Turn(caster, p);
            SpellHelper.GetSurfaceTop(ref p);

            WoodSpirit woodSpirit = new WoodSpirit();
            woodSpirit.Controlled = true;
            woodSpirit.ControlMaster = caster;
            woodSpirit.IsBonded = true;

            Point3D loc = new Point3D(p);
            woodSpirit.MoveToWorld(loc, map);

            Effects.SendLocationEffect(loc, map, 0x376A, 10, 1, 1150, 0);
            woodSpirit.PlaySound(0x467); // Spirit arrival sound

            woodSpirit.Say("*The spirit assists with your woodcutting and warns of dangers!*");

            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => woodSpirit.Delete());
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }

    public class WoodSpirit : BaseCreature
    {
        public WoodSpirit() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.4, 0.6)
        {
            Name = "Spectral Wood Spirit";
            Body = 128; // Spectral appearance
            Hue = 1150; // Ghostly color
            BaseSoundID = 0x467;

            SetStr(50);
            SetDex(60);
            SetInt(100);

            SetHits(100);
            SetStam(100);
            SetMana(0);

            VirtualArmor = 30;

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(5.0), AssistWithWoodcutting);
        }

        // 🔧 Serialization constructor (this fixes the warning)
        public WoodSpirit(Serial serial) : base(serial)
        {
        }

        public void AssistWithWoodcutting()
        {
            if (ControlMaster == null || !ControlMaster.Alive || Deleted)
                return;

            // Assist with woodcutting logic
            if (ControlMaster.InRange(this, 5))
            {
                if (ControlMaster.FindItemOnLayer(Layer.TwoHanded) is Item axe && ControlMaster.Target is IPoint3D target)
                {
                    ControlMaster.SendMessage("The wood spirit assists you with your chopping.");
                    ControlMaster.Animate(32, 5, 1, true, false, 0);
                    Effects.PlaySound(ControlMaster.Location, ControlMaster.Map, 0x13E);
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    Mobile danger = GetNearbyDanger(ControlMaster);

                    if (danger != null)
                    {
                        ControlMaster.SendMessage("The wood spirit warns you of nearby danger!");
                        Effects.SendTargetParticles(this, 0x3709, 10, 30, 5052, EffectLayer.Waist);
                        PlaySound(0x1F8);
                    }
                }
            }
        }

        private Mobile GetNearbyDanger(Mobile master)
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m.Combatant == master)
                    return m;
            }

            return null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
