using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a glistening ooze corpse")]
    public class GlisteningOoze : BaseCreature
    {
        private DateTime m_NextDazzlingLight;
        private DateTime m_NextMirageImage;
        private DateTime m_NextLuringShine;
        private DateTime m_NextGlisteningShield;
        private DateTime m_NextSparklingRetreat;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlisteningOoze()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a glistening ooze";
            Body = 51; // Slime body
            Hue = 2384; // Unique hue for sparkle effect
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
        }

        public GlisteningOoze(Serial serial)
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
                    m_NextDazzlingLight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMirageImage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLuringShine = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextGlisteningShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSparklingRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextDazzlingLight)
                {
                    DazzlingLight();
                }

                if (DateTime.UtcNow >= m_NextMirageImage)
                {
                    MirageImage();
                }

                if (DateTime.UtcNow >= m_NextLuringShine)
                {
                    LuringShine();
                }

                if (DateTime.UtcNow >= m_NextGlisteningShield)
                {
                    GlisteningShield();
                }

                if (DateTime.UtcNow >= m_NextSparklingRetreat)
                {
                    SparklingRetreat();
                }
            }
        }

        private void DazzlingLight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Glistening Ooze emits a dazzling light! *");
            PlaySound(0x1F2); // Sound effect for dazzling light

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    if (m is PlayerMobile player)
                    {
                        player.SendMessage("You are blinded and disoriented by the dazzling light!");
                        player.SendMessage("Your accuracy is reduced and your movement is slowed!");
                        player.Damage(Utility.RandomMinMax(5, 10), this); // Apply damage
                    }
                    
                    // Apply debuff: reduce accuracy and slow movement
                    m.SendMessage("You feel your accuracy slipping and your movements slowing!");
                    m.Frozen = true; // Temporary freeze effect
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => m.Frozen = false); // Unfreeze after 2 seconds
                }
            }

            m_NextDazzlingLight = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for DazzlingLight
        }

        private void MirageImage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Glistening Ooze creates a mirage! *");
            PlaySound(0x1F2); // Sound effect for mirage image

            // Create an illusory duplicate
            MirageImageItem mirage = new MirageImageItem();
            mirage.MoveToWorld(Location, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => mirage.Delete());

            m_NextMirageImage = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for MirageImage
        }

        private void LuringShine()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Glistening Ooze projects a mesmerizing light! *");
            PlaySound(0x1F2); // Sound effect for luring shine

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("You are drawn closer by the mesmerizing light!");
                    m.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => 
                    {
                        if (m.InRange(this, 5))
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Damage over time
                        }
                    });
                }
            }

            m_NextLuringShine = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for LuringShine
        }

        private void GlisteningShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Glistening Ooze activates a glistening shield! *");
            PlaySound(0x1F2); // Sound effect for glistening shield

            // Boost resistance and reflect damage
            this.VirtualArmor = 60;
            this.SetResistance(ResistanceType.Physical, 50, 60);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The glistening shield fades away! *");
                this.VirtualArmor = 40; // Reset armor
                this.SetResistance(ResistanceType.Physical, 40, 50); // Reset resistance
            });

            m_NextGlisteningShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for GlisteningShield
        }

        private void SparklingRetreat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Glistening Ooze makes a sparkling retreat! *");
            PlaySound(0x1F2); // Sound effect for retreat

            // Teleport the ooze a short distance
            Point3D newLocation = Location;
            int xOffset = Utility.RandomMinMax(-5, 5);
            int yOffset = Utility.RandomMinMax(-5, 5);
            newLocation.X += xOffset;
            newLocation.Y += yOffset;

            if (Map.CanFit(newLocation, 16, false, false))
            {
                Location = newLocation;
                Effects.SendLocationEffect(newLocation, Map, 0x3709, 10, 10);
            }

            m_NextSparklingRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for SparklingRetreat
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
            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }

    public class MirageImageItem : Item
    {
        public MirageImageItem() : base(0x171D) // Placeholder item ID
        {
            Movable = false;
        }

        public MirageImageItem(Serial serial) : base(serial)
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
