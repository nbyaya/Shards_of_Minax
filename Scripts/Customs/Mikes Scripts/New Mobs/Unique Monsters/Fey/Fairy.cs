using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Fairy corpse")]
    public class Fairy : BaseCreature
    {
        private DateTime m_NextFairyDust;
        private DateTime m_NextQuickFeet;
        private DateTime m_NextIllusion;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Fairy()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Fairy";
            Body = 128; // GreenGoblin body
            BaseSoundID = 0x467; // Fairy sound
            Hue = 1589; // Bright pink hue

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

            // Initialize abilities
            m_AbilitiesInitialized = false;
        }

        public Fairy(Serial serial)
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
                    m_NextFairyDust = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextQuickFeet = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFairyDust)
                {
                    DoFairyDust();
                }

                if (DateTime.UtcNow >= m_NextQuickFeet)
                {
                    DoQuickFeet();
                }

                if (DateTime.UtcNow >= m_NextIllusion)
                {
                    DoIllusion();
                }
            }
        }

        private void DoFairyDust()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Throws Fairy dust *");
            PlaySound(0x1E5);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(1070853); // The creature's movements become jerky and erratic as it tries to avoid the Fairy dust.
                    m.Damage(Utility.Dice(2, 10, 0), this);
                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                    StatMod mod = new StatMod(StatType.Dex, "FairyDust_Dex", -20, TimeSpan.FromSeconds(10));
                    m.AddStatMod(mod);
                }
            }

            m_NextFairyDust = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void DoQuickFeet()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Moves with incredible speed *");
            PlaySound(0x217);

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            
            ActiveSpeed = 0.1;
            PassiveSpeed = 0.2;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate()
            {
                ActiveSpeed = 0.2;
                PassiveSpeed = 0.4;
            }));

            m_NextQuickFeet = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
        }

        private void DoIllusion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Creates dazzling illusions *");
            PlaySound(0x217);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(3);

                if (loc != Point3D.Zero)
                {
                    Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                    FairyIllusion illusion = new FairyIllusion(this);
                    illusion.MoveToWorld(loc, Map);

                    Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(delegate()
                    {
                        if (!illusion.Deleted)
                            illusion.Delete();
                    }));
                }
            }

            m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
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
            return 0x1F6;
        }

        public override int GetIdleSound()
        {
            return 0x1F7;
        }

        public override int GetAttackSound()
        {
            return 0x1F8;
        }

        public override int GetHurtSound()
        {
            return 0x1F9;
        }

        public override int GetDeathSound()
        {
            return 0x1FA;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            // Serialize the next ability times
            writer.Write(m_NextFairyDust);
            writer.Write(m_NextQuickFeet);
            writer.Write(m_NextIllusion);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Deserialize the next ability times
            m_NextFairyDust = reader.ReadDateTime();
            m_NextQuickFeet = reader.ReadDateTime();
            m_NextIllusion = reader.ReadDateTime();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class FairyIllusion : BaseCreature
    {
        private Mobile m_Owner;

        public FairyIllusion(Mobile owner)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Owner = owner;

            Body = owner.Body;
            Hue = owner.Hue;
            Name = owner.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public FairyIllusion(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Owner == null || m_Owner.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Owner.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
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
