using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Douglas squirrel corpse")]
    public class DouglasSquirrel : BaseCreature
    {
        private DateTime m_NextEarthquake;
        private DateTime m_NextNutShield;
        private DateTime m_NextNutStorm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public DouglasSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Douglas Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2438; // Brown hue

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
        }

        public DouglasSquirrel(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextNutShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNutStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEarthquake)
                {
                    Earthquake();
                }

                if (DateTime.UtcNow >= m_NextNutShield)
                {
                    NutShield();
                }

                if (DateTime.UtcNow >= m_NextNutStorm)
                {
                    NutStorm();
                }
            }
        }

        private void Earthquake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Douglas Squirrel causes the ground to shake violently! *");
            PlaySound(0x20F); // Earthquake sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0); // Pure physical damage

                    // Ensure m is a Mobile before calling SendMessage
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("The ground shakes violently, and you are thrown off balance!");
                    }

                    // Knockback effect
                    if (m is PlayerMobile player)
                    {
                        player.MoveToWorld(new Point3D(player.X + Utility.RandomMinMax(-2, 2), player.Y + Utility.RandomMinMax(-2, 2), player.Z), Map);
                    }
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x13E4, 30); // Ground ripple effect

            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Earthquake
        }

        private void NutShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Douglas Squirrel creates a protective barrier of glowing acorns! *");
            PlaySound(0x1F5); // Shield sound

            // Create Nut Shield Item
            NutShieldItem shield = new NutShieldItem();
            shield.MoveToWorld(Location, Map);

            // Spawn Nut Minions
            for (int i = 0; i < 3; i++)
            {
                NutMinion minion = new NutMinion();
                minion.MoveToWorld(Location, Map);

                // Ensure Combatant is a valid Mobile
                if (Combatant is Mobile target)
                {
                    minion.Combatant = target;
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => shield.Delete()); // Shield lasts for 5 seconds

            m_NextNutShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for NutShield
        }

        private void NutStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Douglas Squirrel unleashes a storm of acorns! *");
            PlaySound(0x20F); // Acorn storm sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0); // Pure physical damage

                    // Ensure m is a Mobile before calling SendMessage
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are pelted by a storm of acorns!");
                        mobile.SendMessage("You feel slightly dazed by the storm!");
                    }
                    
                    m.Damage(0, this);
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x13E5, 30); // Acorn storm effect

            m_NextNutStorm = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for NutStorm
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
            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }

    public class NutShieldItem : Item
    {
        public NutShieldItem() : base(0x1A9C)
        {
            Movable = false;
        }

        public NutShieldItem(Serial serial) : base(serial)
        {
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
        }
    }

    public class NutMinion : BaseCreature
    {
        [Constructable]
        public NutMinion() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Nut Minion";
            Body = 0x116; // Squirrel body
            Hue = 1123; // Brown hue

            SetStr(20, 30);
            SetDex(20);
            SetInt(5);

            SetHits(20, 30);

            SetDamage(4, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);

            SetSkill(SkillName.MagicResist, 10.0);
            SetSkill(SkillName.Tactics, 20.0);
            SetSkill(SkillName.Wrestling, 20.0);

            Fame = 1500;
            Karma = -1500;

            Tamable = false;
        }

        public NutMinion(Serial serial) : base(serial)
        {
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
        }
    }
}
