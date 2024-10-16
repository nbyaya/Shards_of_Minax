using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class ShadowCloneSpell : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Clone", "Kage Bunshin",
            21004, // Icon ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ShadowCloneSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(ShadowClone)))
            {
                Caster.SendMessage("You cannot cast that spell right now.");
                return;
            }

            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x374A, 10, 15, 5030, EffectLayer.Waist);
                Caster.PlaySound(0x482);

                BaseCreature clone = new ShadowClone(Caster);
                clone.MoveToWorld(Caster.Location, Caster.Map);

                Caster.SendMessage("A shadow clone has been created!");

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    if (clone != null && !clone.Deleted)
                        clone.Delete();
                });
            }

            // No FinishSequence method; remove or add custom logic if needed
        }

        private class InternalTarget : Target
        {
            private ShadowCloneSpell m_Owner;

            public InternalTarget(ShadowCloneSpell owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                // No FinishSequence method; handle any cleanup if needed
            }
        }
    }

    public class ShadowClone : BaseCreature
    {
        private Mobile m_Master;

        public ShadowClone(Mobile master) : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = 0;
            Name = $"{master.Name}'s Shadow Clone";

            SetStr(master.Str);
            SetDex(master.Dex);
            SetInt(master.Int);

            if (master is BaseCreature creature)
            {
                SetHits((int)(creature.HitsMax * 0.5));
                SetDamage(creature.DamageMin, creature.DamageMax);
            }
            else
            {
                SetHits((int)(master.HitsMax * 0.5));
                SetDamage(1, 5); // Default damage values if master is not a BaseCreature
            }

            ControlSlots = 1;
            ControlMaster = master;
            Controlled = true;
            // BardImmune = true; // Removed since BardImmune is read-only

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete);
        }

        public override bool OnBeforeDeath()
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            Effects.PlaySound(Location, Map, 0x1FE);
            return base.OnBeforeDeath();
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && from != this && !Deleted)
            {
                int absorb = (int)(amount * 0.3); // Clone absorbs 30% of incoming damage
                amount -= absorb;
                from.Damage(absorb, this);

                base.OnDamage(amount, from, willKill);
            }
        }

        public ShadowClone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
