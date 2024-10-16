using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a nymph corpse")]
    public class Nymph : BaseCreature
    {
        private DateTime m_NextCharm;
        private DateTime m_NextEnchant;
        private DateTime m_NextMistVeil;
        private List<Mobile> m_CharmedPlayers;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Nymph()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nymph";
            Body = 0x191; // GreenGoblin body
            BaseSoundID = 0x4B0; // Pixie sound
            Hue = 1587; // Light green hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            PackItem(new Bandage(Utility.RandomMinMax(5, 10)));
            PackItem(new Apple(Utility.RandomMinMax(3, 5)));

            m_CharmedPlayers = new List<Mobile>();
            m_AbilitiesInitialized = false; // Flag to track if abilities have been initialized
        }

        public Nymph(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}		
        public override Poison PoisonImmune { get { return Poison.Regular; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnchant = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMistVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCharm)
                {
                    DoCharm();
                }

                if (DateTime.UtcNow >= m_NextEnchant)
                {
                    DoEnchant();
                }

                if (DateTime.UtcNow >= m_NextMistVeil)
                {
                    DoMistVeil();
                }
            }
        }

        private void DoCharm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Casts an enchanting spell *");
            PlaySound(0x1ED);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && !m_CharmedPlayers.Contains(m) && m.Combatant == this)
                {
                    m.SendLocalizedMessage(1042072); // You are charmed by the nymph's beauty and feel compelled to protect her.
                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    m.PlaySound(0x1ED);

                    m.Combatant = null;
                    m.Criminal = true;
                    m_CharmedPlayers.Add(m);

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                    {
                        if (m_CharmedPlayers.Contains(m))
                        {
                            m.SendLocalizedMessage(1042073); // You snap out of your trance.
                            m_CharmedPlayers.Remove(m);
                        }
                    }));
                }
            }

            m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
        }

        private void DoEnchant()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Casts an enchantment spell *");
            PlaySound(0x1ED);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && (m is BaseCreature || m_CharmedPlayers.Contains(m)))
                {
                    m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    m.PlaySound(0x1EA);

                    Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate()
                    {
                        m.AddStatMod(new StatMod(StatType.Str, "NymphEnchant_Str", 10, TimeSpan.FromSeconds(30)));
                        m.AddStatMod(new StatMod(StatType.Dex, "NymphEnchant_Dex", 10, TimeSpan.FromSeconds(30)));
                        m.AddStatMod(new StatMod(StatType.Int, "NymphEnchant_Int", 10, TimeSpan.FromSeconds(30)));
                    }));
                }
            }

            m_NextEnchant = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(90, 120));
        }

        private void DoMistVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Summons a thick mist *");
            PlaySound(0x10B);

            for (int i = 0; i < 5; i++)
            {
                Point3D loc = GetSpawnPosition(3);

                if (loc != Point3D.Zero)
                {
                    Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                    MistVeil mist = new MistVeil(this);
                    mist.MoveToWorld(loc, Map);

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                    {
                        if (!mist.Deleted)
                            mist.Delete();
                    }));
                }
            }

            m_NextMistVeil = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 180));
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }

        public override int GetAngerSound()
        {
            return 0x46D;
        }

        public override int GetIdleSound()
        {
            return 0x474;
        }

        public override int GetAttackSound()
        {
            return 0x46B;
        }

        public override int GetHurtSound()
        {
            return 0x470;
        }

        public override int GetDeathSound()
        {
            return 0x471;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = reader.ReadBool();

            if (!m_AbilitiesInitialized)
            {
                // Initialize random cooldowns if not already done
                Random rand = new Random();
                m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 91));
                m_NextEnchant = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(90, 121));
                m_NextMistVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(120, 181));
                m_AbilitiesInitialized = true;
            }

            m_CharmedPlayers = new List<Mobile>();
        }
    }

    public class MistVeil : Item
    {
        private Mobile m_Owner;

        public MistVeil(Mobile owner)
            : base(0x3728)
        {
            m_Owner = owner;
            Movable = false;
        }

        public MistVeil(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
        }
    }
}
