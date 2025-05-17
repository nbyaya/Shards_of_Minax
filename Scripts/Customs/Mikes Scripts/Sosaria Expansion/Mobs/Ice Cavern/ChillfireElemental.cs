using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a chillfire elemental corpse")]
    public class ChillfireElemental : BaseCreature
    {
        private DateTime m_NextBurst;
        private DateTime m_NextPulse;
        private DateTime m_NextField;
        private bool m_Initialized;

        [Constructable]
        public ChillfireElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Chillfire Elemental";
            Body = 15;
            BaseSoundID = 838;
            Hue = 2972; // Chilling blue flame hue


            SetStr(200, 240);
            SetDex(150, 180);
            SetInt(250, 300);

            SetHits(450, 580);

            SetDamage(18, 26);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 45);
            SetDamageType(ResistanceType.Cold, 45);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            ControlSlots = 4;

            PackItem(new BlackPearl(3));
            PackItem(new SulfurousAsh(3));

            AddItem(new LightSource());
        }

        public ChillfireElemental(Serial serial) : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override double DispelDifficulty => 120.5;
        public override double DispelFocus => 50.0;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_Initialized)
            {
                m_NextBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                m_NextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
                m_NextField = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
                m_Initialized = true;
            }

            if (DateTime.UtcNow >= m_NextBurst)
                BurstAttack();

            if (DateTime.UtcNow >= m_NextPulse)
                AuraPulse();

            if (DateTime.UtcNow >= m_NextField)
                IcefireField();
        }

        private void BurstAttack()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Unleashes a burst of freezing flame!*");
            PlaySound(0x208);
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(25, 40), 0, 50, 50, 0, 0);

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage(38, "You're engulfed in an icy inferno!");
                        mobile.Freeze(TimeSpan.FromSeconds(1.5));
                    }
                }
            }

            m_NextBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void AuraPulse()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Pulses with unstable elemental energy!*");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, Hue, 0, 5029, 0);

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 20), 0, 50, 50, 0, 0);
                }
            }

            m_NextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
        }

        private void IcefireField()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Creates a field of freezing fire!*");

            for (int i = 0; i < 6; i++)
            {
                Point3D point = new Point3D(Location.X + Utility.RandomMinMax(-2, 2), Location.Y + Utility.RandomMinMax(-2, 2), Location.Z);
                Map map = Map;

                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 300), () =>
                {
                    if (map != null)
                    {
                        Effects.SendLocationEffect(point, map, 0x36BD, 30, 10, Hue);
                        PlaySound(0x208);

                        foreach (Mobile m in map.GetMobilesInRange(point, 1))
                        {
                            if (m != this && m.Alive && !m.IsDeadBondedPet)
                            {
                                AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 50, 50, 0, 0);

                                if (m is Mobile target)
                                {
                                    target.SendMessage("The chillfire scorches and freezes you simultaneously!");
                                }
                            }
                        }
                    }
                });
            }

            m_NextField = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
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
            m_Initialized = false;
        }
    }
}
