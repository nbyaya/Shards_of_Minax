using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost ogre corpse")]
    public class FrostOgre : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextIceShard;
        private DateTime m_NextFrostArmor;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost ogre";
            Body = 1; // Ogre body
            Hue = 2175; // Light blue hue for icy effect
            BaseSoundID = 427;

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

        public FrostOgre(Serial serial)
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
                    m_NextIceShard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextIceShard)
                {
                    IceShard();
                }

                if (DateTime.UtcNow >= m_NextFrostArmor)
                {
                    FrostArmor();
                }
            }
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Breathes a cone of icy air *");
            PlaySound(0x20F); // Custom sound for frost breath
            FixedEffect(0x376A, 10, 16); // Visual effect for frost breath

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && InLOS(m))
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    m.Damage(damage, this);
                    m.SendMessage("You are struck by a chilling frost breath!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Slow down the target
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for FrostBreath
        }

        private void IceShard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Launches shards of ice *");
            PlaySound(0x20F); // Custom sound for ice shards
            FixedEffect(0x376A, 10, 16); // Visual effect for ice shards

            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(8, 12);
                target.Damage(damage, this);
                target.SendMessage("You are hit by a sharp shard of ice!");
                target.Freeze(TimeSpan.FromSeconds(2)); // Chance to freeze
            }

            m_NextIceShard = DateTime.UtcNow + TimeSpan.FromSeconds(35); // Cooldown for IceShard
        }

        private void FrostArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Envelopes itself in a frosty shield *");
            PlaySound(0x20F); // Custom sound for frost armor
            FixedEffect(0x376A, 10, 16); // Visual effect for frost armor

            this.VirtualArmor += 20;
            this.SendMessage("The Frost Ogre's icy shield absorbs some of the damage!");

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                if (!Deleted)
                {
                    this.VirtualArmor -= 20; // Remove the armor effect after some time
                }
            });

            m_NextFrostArmor = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for FrostArmor
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
