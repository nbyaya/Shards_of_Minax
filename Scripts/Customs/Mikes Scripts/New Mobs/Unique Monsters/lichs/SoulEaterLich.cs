using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a soul eater lich's corpse")]
    public class SoulEaterLich : Lich
    {
        private DateTime m_NextSoulDrain;
        private DateTime m_NextPhantomStrikes;
        private DateTime m_NextSoulHarvest;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SoulEaterLich()
            : base()
        {
            Name = "a soul eater lich";
            Hue = 2134; // Unique hue for Soul Eater Lich
			BaseSoundID = 0x3E9;

            // Increase stats and adjust resistances and skills as needed
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

        public SoulEaterLich(Serial serial)
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
                    m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPhantomStrikes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSoulHarvest = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSoulDrain)
                {
                    SoulDrain();
                }

                if (DateTime.UtcNow >= m_NextPhantomStrikes)
                {
                    PhantomStrikes();
                }
            }

            if (DateTime.UtcNow >= m_NextSoulHarvest)
            {
                SoulHarvest();
            }
        }

        private void SoulDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + damage, HitsMax);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soul Drain! *");
                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);
                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private void PhantomStrikes()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                target.SendMessage("Your hit points feel momentarily out of your control!");
                // Here you can implement a more complex effect, but let's use a simple delay
                Timer.DelayCall(TimeSpan.FromSeconds(3), () => 
                {
                    if (target.Alive)
                    {
                        target.SendMessage("You regain control over your hit points.");
                    }
                });
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Phantom Strikes! *");
                m_NextPhantomStrikes = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void SoulHarvest()
        {
            if (Combatant == null || !(Combatant is PlayerMobile))
                return;

            PlayerMobile player = Combatant as PlayerMobile;
            if (player != null)
            {
                player.SendMessage("Your stats have been temporarily weakened by the Soul Eater Lich's death throes!");
                // Reduce player's stats for a short time
                player.Str -= 10;
                player.Dex -= 10;
                player.Int -= 10;
                Timer.DelayCall(TimeSpan.FromMinutes(1), () => 
                {
                    if (player != null && !player.Deleted)
                    {
                        player.Str += 10;
                        player.Dex += 10;
                        player.Int += 10;
                        player.SendMessage("Your stats have returned to normal.");
                    }
                });
            }
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soul Harvest! *");
            m_NextSoulHarvest = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
