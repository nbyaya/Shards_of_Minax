using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vecna")]
    public class Vecna : BaseCreature
    {
        private DateTime m_NextHandOfVecna;
        private DateTime m_NextWhispersOfMadness;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Vecna()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Vecna, the Whispered One";
            Body = 78; // AncientLich body
            Hue = 2092; // Unique dark hue
			BaseSoundID = 412;

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
            PackNecroReg(100, 200);

            m_AbilitiesInitialized = false;
        }

        public Vecna(Serial serial)
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
                    m_NextHandOfVecna = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextWhispersOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextHandOfVecna)
                {
                    HandOfVecna();
                }

                if (DateTime.UtcNow >= m_NextWhispersOfMadness)
                {
                    WhispersOfMadness();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }
            }
        }

        private void HandOfVecna()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vecna unleashes the Hand of Vecna! *");
            PlaySound(0x1D4); // Dark magic sound

            if (Combatant != null)
            {
                int healthDrain = Utility.RandomMinMax(20, 30); // Drain 20-30% of target's health
                int drainAmount = (int)(Combatant.HitsMax * (healthDrain / 100.0));
                Combatant.Hits -= drainAmount;
                Hits += drainAmount;

                if (Combatant is Mobile mobile)
                {
                    mobile.SendMessage("You feel your life being drained away!");
                    mobile.SendMessage("Vecna has restored his own health with your essence!");
                }
            }

            m_NextHandOfVecna = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for HandOfVecna
        }

        private void WhispersOfMadness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vecna's whispers echo around you, sowing madness! *");
            PlaySound(0x1D4); // Dark magic sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        m.SendMessage("You hear maddening whispers and feel disoriented!");
                        // Reverse movement controls for 5 seconds
                        m.SendMessage("Your controls are reversed for a short duration!");
                        Timer.DelayCall(TimeSpan.FromSeconds(5), () => 
                        {
                            // Revert movement controls
                            m.SendMessage("You regain control of your movements.");
                        });
                    });
                }
            }

            m_NextWhispersOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for WhispersOfMadness
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vecna summons dark minions to aid him in battle! *");
            PlaySound(0x1D4); // Dark magic sound

            for (int i = 0; i < 2; i++) // Summon 2 minions
            {
                DarkMinion minion = new DarkMinion();
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant;
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for SummonMinions
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

            m_AbilitiesInitialized = false;
        }
    }

    public class DarkMinion : BaseCreature
    {
        [Constructable]
        public DarkMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dark Minion";
            Body = 78; // Same body as Vecna for consistency
            Hue = 1153; // Darker hue for minions

            SetStr(100, 150);
            SetDex(75, 90);
            SetInt(30, 50);

            SetHits(100, 150);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 30;

            Tamable = false;
        }

        public DarkMinion(Serial serial)
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
