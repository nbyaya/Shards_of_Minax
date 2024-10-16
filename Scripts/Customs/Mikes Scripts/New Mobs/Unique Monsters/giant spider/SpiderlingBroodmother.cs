using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a spiderling broodmother corpse")]
    public class SpiderlingMinionBroodmother : BaseCreature
    {
        private static readonly int[] SpiderlingMinionTypes = new int[] { 28 }; // Using the same body as GiantSpider
        private static readonly int[] SpiderlingMinionHues = new int[] { 1150, 1151, 1152 }; // Different hues for variety

        private DateTime m_NextSpawnBroodling;
        private DateTime m_NextWebOverload;
        private DateTime m_NextSilkenCocoon;
        private DateTime m_NextToxicBurst;
        private DateTime m_NextReinforcements;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SpiderlingMinionBroodmother()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Spiderling Minion Broodmother";
            Body = 28; // Same body as GiantSpider
            Hue = 1785; // Unique hue
			BaseSoundID = 0x388;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SpiderlingMinionBroodmother(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSpawnBroodling = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWebOverload = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSilkenCocoon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 70));
                    m_NextReinforcements = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 80));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSpawnBroodling)
                {
                    SpawnBroodlings();
                }

                if (DateTime.UtcNow >= m_NextWebOverload)
                {
                    WebOverload();
                }

                if (DateTime.UtcNow >= m_NextSilkenCocoon)
                {
                    SilkenCocoon();
                }

                if (DateTime.UtcNow >= m_NextToxicBurst && Hits < HitsMax * 0.3)
                {
                    ToxicBurst();
                }

                if (DateTime.UtcNow >= m_NextReinforcements && Hits < HitsMax * 0.5)
                {
                    CallForReinforcements();
                }
            }
        }

        private void SpawnBroodlings()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother spawns spiderlings! *");

            for (int i = 0; i < 3; i++)
            {
                int index = Utility.Random(SpiderlingMinionTypes.Length);
                SpiderlingMinion spiderling = new SpiderlingMinion();
                spiderling.Body = SpiderlingMinionTypes[index];
                spiderling.Hue = SpiderlingMinionHues[Utility.Random(SpiderlingMinionHues.Length)];
                spiderling.MoveToWorld(Location, Map);
            }

            m_NextSpawnBroodling = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void WebOverload()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother releases a web overload! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are caught in the Broodmother's web!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 100, 0, 0);
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => CreateWebArea());

            m_NextWebOverload = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void CreateWebArea()
        {
            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 16);
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are ensnared by the Broodmother's web area!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 100, 0, 0);
                }
            }
        }

        private void SilkenCocoon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother wraps itself in a silken cocoon! *");

            int healAmount = 70;
            Hits += healAmount;
            if (Hits > HitsMax)
                Hits = HitsMax;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(EndCocoon));

            m_NextSilkenCocoon = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void EndCocoon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother emerges from the cocoon! *");

            // Launch a web-based attack after emerging
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("The Broodmother's emergence triggers a web attack!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 100, 0, 0);
                }
            }
        }

        private void ToxicBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother releases a burst of toxic web! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are hit by a toxic burst!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0, 0);
                    m.ApplyPoison(this, Poison.Greater);
                }
            }

            m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void CallForReinforcements()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Broodmother calls for reinforcements! *");

            for (int i = 0; i < 2; i++)
            {
                int index = Utility.Random(SpiderlingMinionTypes.Length);
                SpiderlingMinion spiderling = new SpiderlingMinion();
                spiderling.Body = SpiderlingMinionTypes[index];
                spiderling.Hue = SpiderlingMinionHues[Utility.Random(SpiderlingMinionHues.Length)];
                spiderling.MoveToWorld(Location, Map);
            }

            m_NextReinforcements = DateTime.UtcNow + TimeSpan.FromSeconds(90);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class SpiderlingMinion : BaseCreature
    {
        [Constructable]
        public SpiderlingMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spiderling minion";
            Body = 28;
            Hue = 1150;

            SetStr(50);
            SetDex(50);
            SetInt(20);

            SetHits(30);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);

            SetSkill(SkillName.Poisoning, 40.0, 60.0);
            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 30.0, 50.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 10;

            Tamable = false;
        }

        public SpiderlingMinion(Serial serial)
            : base(serial)
        {
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
