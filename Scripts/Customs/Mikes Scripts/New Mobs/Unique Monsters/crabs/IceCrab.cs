using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ice crab corpse")]
    public class IceCrab : BaseMount
    {
        private DateTime m_NextFrostyGrasp;
        private DateTime m_NextGlacialSmash;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IceCrab()
            : this("Ice Crab")
        {
        }

        [Constructable]
        public IceCrab(string name)
            : base(name, 1510, 16081, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F2;
            Body = 1510; // Coconut Crab body
            Hue = 1459; // Ice blue hue

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
            SetResistance(ResistanceType.Poison, 100);
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

            // Initialize abilities with a placeholder
            m_NextFrostyGrasp = DateTime.UtcNow;
            m_NextGlacialSmash = DateTime.UtcNow;

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public IceCrab(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextFrostyGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGlacialSmash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostyGrasp)
                {
                    FrostyGrasp();
                }

                if (DateTime.UtcNow >= m_NextGlacialSmash)
                {
                    GlacialSmash();
                }
            }
        }

        private void FrostyGrasp()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ice Crab uses Frosty Grasp! *");
            Effects.PlaySound(Combatant.Location, Map, 0x0F5); // Ice sound

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
            {
                if (Combatant == null || !Combatant.Alive)
                    return;

                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.Freeze(TimeSpan.FromSeconds(10));
                    target.SendMessage("You feel a chilling grip slowing your movements!");
                }
            }));

            m_NextFrostyGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        private void GlacialSmash()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ice Crab performs a Glacial Smash! *");
            Effects.PlaySound(Combatant.Location, Map, 0x0F6); // Ice shattering sound

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
            {
                if (Combatant == null || !Combatant.Alive)
                    return;

                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.SendMessage("The Ice Crab's attack hits you with freezing cold!");
                    target.Freeze(TimeSpan.FromSeconds(10)); // Freeze effect if slowed
                }
            }));

            m_NextGlacialSmash = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Fixed cooldown
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

            // Reset the initialization flag
            m_AbilitiesInitialized = false;
        }
    }
}
