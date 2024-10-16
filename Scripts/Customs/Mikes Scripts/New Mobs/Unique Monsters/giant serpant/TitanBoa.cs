using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a titan boa corpse")]
    public class TitanBoa : GiantSerpent
    {
        private DateTime m_NextCrushingEmbrace;
        private DateTime m_NextTerrifyingRoar;
        private DateTime m_NextGroundShaker;
        private DateTime m_NextVenomSpit;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TitanBoa()
            : base()
        {
            Name = "a Titan Boa";
            Body = 0x15; // Giant Serpent body
            Hue = 1771; // Unique hue
			BaseSoundID = 219;
            
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

        public TitanBoa(Serial serial)
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
                    m_NextCrushingEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTerrifyingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGroundShaker = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVenomSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrushingEmbrace)
                {
                    CrushingEmbrace();
                }

                if (DateTime.UtcNow >= m_NextTerrifyingRoar)
                {
                    TerrifyingRoar();
                }

                if (DateTime.UtcNow >= m_NextGroundShaker)
                {
                    GroundShaker();
                }

                if (DateTime.UtcNow >= m_NextVenomSpit)
                {
                    VenomSpit();
                }
            }
        }

        private void CrushingEmbrace()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Titan Boa constricts its foe with a crushing embrace! *");

            Mobile target = Combatant as Mobile;
            if (target != null && InRange(target, 1))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1), delegate
                {
                    if (target != null && !target.Deleted && InRange(target, 1))
                    {
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(target, this, damage, 0, 0, 0, 0, 0);
                        target.SendMessage("You are squeezed painfully by the Titan Boa!");
                    }
                });
            }

            m_NextCrushingEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for Crushing Embrace
        }

        private void TerrifyingRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Titan Boa lets out a bone-chilling roar! *");

            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m != this && m.Player && !m.InRange(this, 5))
                {
                    m.SendMessage("The Titan Boa's roar fills you with terror!");
                }
                else if (m != this && m.Player && m.InRange(this, 5))
                {
                    m.SendMessage("You are momentarily stunned by the Titan Boa's roar!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Adds a brief stun effect
                }
            }

            m_NextTerrifyingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Terrifying Roar
        }

        private void GroundShaker()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Titan Boa causes the ground to tremble violently! *");

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);
                    m.SendMessage("The ground shakes beneath you, causing great pain!");
                }
            }

            m_NextGroundShaker = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Ground Shaker
        }

        private void VenomSpit()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Titan Boa spits venom at its enemies! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);
                    m.SendMessage("You are hit by the Titan Boa's venomous spit!");

                    // Apply poison effect
                    m.ApplyPoison(this, Poison.Lethal);
                }
            }

            m_NextVenomSpit = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Venom Spit
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

            // Reset ability timers and flag for initialization
            m_AbilitiesInitialized = false;
        }
    }
}
