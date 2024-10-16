using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a stonebreath corpse")]
    public class Mordrake : BaseCreature
    {
        private DateTime m_NextPetrifyingGaze;
        private DateTime m_NextStoneAura;
        private DateTime m_NextCrushingWeight;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Mordrake()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Mordrake the Stonebreath";
            Body = 4; // Gargoyle body
            Hue = 1150; // Custom hue for uniqueness
            BaseSoundID = 372;

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

        public Mordrake(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override bool CanFly { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPetrifyingGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStoneAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextCrushingWeight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPetrifyingGaze)
                {
                    PetrifyingGaze();
                }

                if (DateTime.UtcNow >= m_NextStoneAura)
                {
                    StoneAura();
                }

                if (DateTime.UtcNow >= m_NextCrushingWeight)
                {
                    CrushingWeight();
                }
            }
        }

        private void PetrifyingGaze()
        {
            if (Combatant != null && Utility.RandomDouble() < 0.20) // Increased chance
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("Mordrake's petrifying gaze turns you into stone!");
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Petrifying Gaze! *");
                    target.Freeze(TimeSpan.FromSeconds(5)); // Longer freeze
                    target.FixedEffect(0x376A, 10, 16); // Visual effect
                }

                m_NextPetrifyingGaze = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reduced cooldown
            }
        }

        private void StoneAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Stone Aura activated! *");
            FixedEffect(0x373A, 10, 16); // Visual effect
            VirtualArmor += 30; // Increase physical resistance

            // Damage and reflect
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(20, this); // Area damage
                    if (Utility.RandomDouble() < 0.25) // 25% chance to reflect damage
                    {
                        this.Damage(10, m);
                        this.SendMessage("Stone Aura reflects some of the damage back at you!");
                    }
                }
            }

            m_NextStoneAura = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown
        }

        private void CrushingWeight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Crushing Weight! *");
            PlaySound(0x3D9); // Sound effect
            FixedEffect(0x376A, 10, 16); // Visual effect

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(40, this); // Increased damage
                    m.SendMessage("You are crushed by Mordrake's weight!");
                    m.FixedEffect(0x376A, 10, 16); // Visual effect
                    AddDebuff(m, "Stunned", TimeSpan.FromSeconds(5)); // Stun debuff
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z); // Knockback
                }
            }

            m_NextCrushingWeight = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown
        }

        private void AddDebuff(Mobile m, string message, TimeSpan duration)
        {
            m.SendMessage(message);
            Timer.DelayCall(duration, delegate 
            {
                if (m.Alive) 
                    m.SendMessage("The effect has worn off.");
            });
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
