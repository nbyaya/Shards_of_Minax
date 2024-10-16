using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost warden ettin corpse")]
    public class FrostWardenEttin : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextIceBarrier;
        private DateTime m_NextFrostbiteAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostWardenEttin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost warden ettin";
            Body = 18; // Same as Ettin
            Hue = 1565; // Icy blue hue
			BaseSoundID = 367;

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

        public FrostWardenEttin(Serial serial)
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
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostbiteAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextIceBarrier)
                {
                    IceBarrier();
                }

                if (DateTime.UtcNow >= m_NextFrostbiteAura)
                {
                    FrostbiteAura();
                }
            }
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Warden Ettin exhales a chilling breath! *");
            PlaySound(0x1F8); // Frost breath sound
            FixedEffect(0x37C4, 10, 16); // Frost breath effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel a chilling cold slowing your movements!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown
        }

        private void IceBarrier()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Warden Ettin conjures an ice shield! *");
            PlaySound(0x1F9); // Ice shield sound
            FixedEffect(0x376A, 10, 16); // Ice shield effect

            this.VirtualArmor += 15; // Increase armor temporarily

            Timer.DelayCall(TimeSpan.FromSeconds(15), () => 
            {
                if (!this.Deleted)
                    this.VirtualArmor -= 15; // Reset armor after duration
            });

            m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update cooldown
        }

        private void FrostbiteAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frost Warden Ettin emanates a frosty aura! *");
            PlaySound(0x1F8); // Frostbite aura sound
            FixedEffect(0x376A, 10, 16); // Frostbite aura effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are afflicted by the Frostbite Aura!");
                    m.Damage(5, this);
                }
            }

            m_NextFrostbiteAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Update cooldown
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
