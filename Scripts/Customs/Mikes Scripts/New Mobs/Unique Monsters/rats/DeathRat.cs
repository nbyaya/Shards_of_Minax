using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death rat corpse")]
    public class DeathRat : BaseCreature
    {
        private DateTime m_NextPestilenceCloud;
        private DateTime m_NextSummonMinions;
        private DateTime m_NextSpecialAttack;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public DeathRat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a death rat";
            Body = 0xD7; // Using Giant Rat body
            Hue = 2268; // Unique greenish hue for the plague effect
            BaseSoundID = 0x188;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public DeathRat(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextPestilenceCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPestilenceCloud)
                {
                    CastPestilenceCloud();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonPlagueMinions();
                }

                if (DateTime.UtcNow >= m_NextSpecialAttack)
                {
                    ExecuteSpecialAttack();
                }
            }
        }

        private void CastPestilenceCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A cloud of pestilence engulfs you! *");
            FixedEffect(0x376A, 10, 16); // Greenish fog effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a cloud of pestilence!");
                    // Reducing health regeneration by 50% and applying poison damage
                    m.Damage(Utility.RandomMinMax(10, 15), this);
                    // Apply poison damage or other effects here
                    if (m.Poison == null)
                        m.ApplyPoison(this, Poison.Greater); // Example of applying poison
                }
            }

            m_NextPestilenceCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SummonPlagueMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Plague Rat summons plague minions! *");
            FixedEffect(0x37B9, 10, 16); // Summoning effect

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = GetSpawnPosition(2);
                if (loc != Point3D.Zero)
                {
                    PlagueMinion minion = new PlagueMinion();
                    minion.MoveToWorld(loc, Map);
                }
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void ExecuteSpecialAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Plague Rat releases a deadly bile! *");
            FixedEffect(0x373A, 10, 16); // Special attack effect

            if (Combatant != null && Combatant.Alive)
            {
                Combatant.Damage(Utility.RandomMinMax(15, 25), this);

                Mobile target = Combatant as Mobile; // Ensure Combatant is a Mobile
                if (target != null)
                {
                    target.SendMessage("You are struck by the Plague Rat's deadly bile!");

                    if (Utility.RandomDouble() < 0.5)
                    {
                        // Apply debuff effects here
                        target.ApplyPoison(this, Poison.Lethal); // Example of applying poison debuff
                    }
                }
            }

            m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

    public class PlagueMinion : BaseCreature
    {
        [Constructable]
        public PlagueMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a plague minion";
            Body = 0xD7; // Using Giant Rat body for minions
            Hue = 0x4B1; // Darker greenish hue for minions
            BaseSoundID = 0x188;

            this.SetStr(20, 40);
            this.SetDex(30, 50);
            this.SetInt(10, 20);

            this.SetHits(15, 30);
            this.SetMana(0);

            this.SetDamage(4, 8);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 15, 25);
            this.SetResistance(ResistanceType.Fire, 5, 10);
            this.SetResistance(ResistanceType.Poison, 20, 30);

            this.SetSkill(SkillName.MagicResist, 10.0, 20.0);
            this.SetSkill(SkillName.Tactics, 15.0, 25.0);
            this.SetSkill(SkillName.Wrestling, 15.0, 25.0);

            this.Fame = 200;
            this.Karma = -200;

            this.VirtualArmor = 15;
        }

        public PlagueMinion(Serial serial)
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
