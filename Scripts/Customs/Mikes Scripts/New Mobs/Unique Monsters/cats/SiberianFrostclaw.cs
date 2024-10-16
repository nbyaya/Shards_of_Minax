using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frostclaw corpse")]
    public class SiberianFrostclaw : BaseCreature
    {
        private DateTime m_NextIceShard;
        private DateTime m_NextFrostbite;
        private DateTime m_NextGlacialRoar;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SiberianFrostclaw()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Siberian Frostclaw";
            Body = 0xC9; // Cat body
            Hue = 1295; // Unique hue
            BaseSoundID = 0x69; // Cat sound ID

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

            // Initialize abilities
            m_AbilitiesInitialized = false;
        }

        public SiberianFrostclaw(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextIceShard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextGlacialRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIceShard)
                {
                    UseIceShard();
                }

                if (DateTime.UtcNow >= m_NextFrostbite)
                {
                    UseFrostbite();
                }

                if (DateTime.UtcNow >= m_NextGlacialRoar)
                {
                    UseGlacialRoar();
                }
            }
        }

        private void UseIceShard()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            Mobile target = Combatant as Mobile;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siberian Frostclaw breathes a freezing gale! *");
            target.FixedEffect(0x376A, 10, 16);
            target.SendMessage("You are struck by a shard of ice!");
            target.Damage(Utility.RandomMinMax(20, 30), this);
            target.Freeze(TimeSpan.FromSeconds(2));

            m_NextIceShard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void UseFrostbite()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            Mobile target = Combatant as Mobile;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siberian Frostclaw emits a chilling frostbite! *");
            target.SendMessage("You feel your strength drain away from the frostbite!");
            target.Damage(Utility.RandomMinMax(15, 20), this);

            m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void UseGlacialRoar()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siberian Frostclaw roars with glacial force! *");
            FixedEffect(0x37C4, 10, 36);
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    m.SendMessage("The icy roar of the Frostclaw chills you to the bone!");
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextGlacialRoar = DateTime.UtcNow + TimeSpan.FromSeconds(35);
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
}
