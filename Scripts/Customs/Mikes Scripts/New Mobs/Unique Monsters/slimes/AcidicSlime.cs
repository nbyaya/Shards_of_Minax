using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an acidic slime corpse")]
    public class AcidicSlime : BaseCreature
    {
        private DateTime m_NextAcidicSpray;
        private DateTime m_NextCorrosiveTouch;
        private DateTime m_NextCorrosivePulse;
        private bool m_AbilitiesInitialized;
        private bool m_IsMelting;

        [Constructable]
        public AcidicSlime()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an acidic slime";
            Body = 51; // Slime body
            Hue = 2396; // Unique acidic green hue
			BaseSoundID = 456;

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

            m_AbilitiesInitialized = false;
            m_IsMelting = false;
        }

        public AcidicSlime(Serial serial)
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
                    m_NextAcidicSpray = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCorrosiveTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextCorrosivePulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextAcidicSpray)
                {
                    AcidicSpray();
                }

                if (DateTime.UtcNow >= m_NextCorrosiveTouch)
                {
                    CorrosiveTouch();
                }

                if (DateTime.UtcNow >= m_NextCorrosivePulse)
                {
                    CorrosivePulse();
                }
            }
        }

        private void AcidicSpray()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Acidic Slime spews a cone of corrosive acid! *");
            PlaySound(0x227); // Acid spray sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && InFront(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    ApplyCorrosionDebuff(m);
                }
            }

            m_NextAcidicSpray = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for AcidicSpray
        }

        private void CorrosiveTouch()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Acidic Slime's touch corrodes your armor! *");
            PlaySound(0x227); // Acid touch sound

            if (Combatant != null)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0);
				
            }

            m_NextCorrosiveTouch = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for CorrosiveTouch
        }

        private void CorrosivePulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Acidic Slime releases a pulse of corrosive energy! *");
            PlaySound(0x227); // Acid pulse sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                    ApplyCorrosionDebuff(m);
                }
            }

            m_NextCorrosivePulse = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for CorrosivePulse
        }

        private bool InFront(Mobile m)
        {
            if (m == null || m.Deleted) return false;

            double angle = (m.Direction - this.Direction) % 8;
            if (angle < 0) angle += 8;

            return angle == 0 || angle == 1 || angle == 7; // Adjust based on what "InFront" means for your case
        }

        private void ApplyCorrosionDebuff(Mobile target)
        {
            if (target is PlayerMobile player)
            {
                foreach (Item item in player.Items)
                {
                    if (item is BaseArmor armor)
                    {
                        // Apply your corrosion effect here
                    }
                    else if (item is BaseWeapon weapon)
                    {
                        // Apply your corrosion effect here
                    }
                }
            }
        }

        private void AcidicExplosion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Acidic Slime explodes in a burst of corrosive goo! *");
            PlaySound(0x307); // Explosion sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                    ApplyCorrosionDebuff(m);
                }
            }

            // Create acid pool on the ground
            Timer.DelayCall(TimeSpan.FromSeconds(1), () => CreateAcidPool());

            // Delete itself after the explosion
            Delete();
        }

        private void CreateAcidPool()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A pool of corrosive acid forms on the ground! *");
            PlaySound(0x1F4); // Acid pool sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 0, 100, 0, 0);
                    m.SendMessage("You are burned by the corrosive acid pool!");
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            AcidicExplosion();
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
