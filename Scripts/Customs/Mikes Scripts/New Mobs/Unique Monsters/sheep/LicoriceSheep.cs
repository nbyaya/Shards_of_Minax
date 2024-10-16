using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a licorice sheep corpse")]
    public class LicoriceSheep : BaseCreature
    {
        private DateTime m_NextLicoriceLash;
        private DateTime m_NextSweetSmell;
        private DateTime m_NextLicoriceTrap;
        private DateTime m_NextLicoriceShield;
        private bool m_AbilitiesInitialized;
        private bool m_LicoriceShieldActive;

        [Constructable]
        public LicoriceSheep()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Licorice Sheep";
            Body = 0xCF; // Sheep body
            Hue = 2347; // Unique hue (purple)
			BaseSoundID = 0xD6;

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
            m_LicoriceShieldActive = false;
        }

        public LicoriceSheep(Serial serial)
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
                    m_NextLicoriceLash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSweetSmell = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLicoriceTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextLicoriceShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLicoriceLash)
                {
                    LicoriceLash();
                }

                if (DateTime.UtcNow >= m_NextSweetSmell)
                {
                    SweetSmell();
                }

                if (DateTime.UtcNow >= m_NextLicoriceTrap)
                {
                    LicoriceTrap();
                }

                if (DateTime.UtcNow >= m_NextLicoriceShield && !m_LicoriceShieldActive)
                {
                    LicoriceShield();
                }
            }

            if (m_LicoriceShieldActive && DateTime.UtcNow >= m_NextLicoriceShield)
            {
                DeactivateLicoriceShield();
            }
        }

        private void LicoriceLash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Licorice Sheep lashes out with its licorice tendrils! *");
            PlaySound(0x2D6); // Unique sound for the lash

            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile; // Cast to Mobile
                int damage = Utility.RandomMinMax(8, 15);
                AOS.Damage(Combatant, this, damage, 0, 0, 100, 0, 0);

                if (Utility.RandomDouble() < 0.35) // 35% chance to cause bleeding
                {
                    target.SendMessage("You are bleeding from the licorice lash!");
                    target.SendMessage("You will take damage every second for a while.");
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (target != null && target.Alive)
                        {
                            target.SendMessage("You are still bleeding from the licorice lash!");
                            AOS.Damage(target, this, 1, 0, 0, 0, 0, 0); // Bleed damage
                        }
                    });
                }
            }

            m_NextLicoriceLash = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void SweetSmell()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Licorice Sheep emits a sweet aroma that confuses its enemies! *");
            PlaySound(0x2D7); // Unique sound for the sweet smell

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("Your accuracy is reduced by the sweet aroma!");
                    m.SendMessage("You also feel slower due to the aroma!");
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        if (m != null && m.Alive)
                        {
                            m.SendMessage("The sweet aroma has worn off, and your accuracy is restored.");
                        }
                    });
                    // Reduce hit chance and movement speed
                    m.SendMessage("You feel slower due to the aroma!");
                    m.Damage((int)(m.Hits * 0.1)); // Apply a small amount of damage
                }
            }

            m_NextSweetSmell = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void LicoriceTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Licorice Sheep sets a licorice trap on the ground! *");
            PlaySound(0x2D8); // Unique sound for the trap

            Point3D trapLocation = Location;
            Trap trap = new Trap();
            trap.MoveToWorld(trapLocation, Map);
            Timer.DelayCall(TimeSpan.FromSeconds(5), () => ExplodeTrap(trap));

            m_NextLicoriceTrap = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void ExplodeTrap(Trap trap)
        {
            if (trap.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The licorice trap explodes, slowing and damaging those nearby! *");
            PlaySound(0x2D9); // Explosion sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in the licorice trap!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 0); // Damage from trap
                    m.SendMessage("You feel slowed by the sticky licorice!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Slow effect
                }
            }

            trap.Delete();
        }

        private void LicoriceShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Licorice Sheep wraps itself in a licorice shield! *");
            PlaySound(0x2DA); // Shield sound

            FixedParticles(0x374A, 9, 32, 5030, EffectLayer.Waist);

            m_LicoriceShieldActive = true;
            VirtualArmor += 20; // Increase virtual armor
            m_NextLicoriceShield = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Shield duration
        }

        private void DeactivateLicoriceShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Licorice Shield fades away! *");
            m_LicoriceShieldActive = false;
            VirtualArmor -= 20; // Revert armor increase
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

        private class Trap : Item
        {
            public Trap() : base(0x1D7B) // Placeholder ID
            {
                Movable = false;
            }

            public Trap(Serial serial) : base(serial)
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
}
