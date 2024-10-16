using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bush pig corpse")]
    public class BushPig : BaseCreature
    {
        private DateTime m_NextCamouflage;
        private DateTime m_NextRazorShards;
        private DateTime m_CamouflageEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BushPig()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bush pig";
            Body = 0xCB; // Pig body
            Hue = 2225; // Brown hue
            BaseSoundID = 0xC4; // Pig sound

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

        public BushPig(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRazorShards = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCamouflage && DateTime.UtcNow >= m_CamouflageEnd)
                {
                    ActivateCamouflage();
                }

                if (DateTime.UtcNow >= m_NextRazorShards)
                {
                    LaunchRazorShards();
                }
            }

            if (DateTime.UtcNow >= m_CamouflageEnd && m_CamouflageEnd != DateTime.MinValue)
            {
                DeactivateCamouflage();
            }
        }

        private void ActivateCamouflage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blends into the foliage *");
            PlaySound(0x204); // Rustling sound
            FixedEffect(0x376A, 10, 16);

            Hidden = true;
            m_CamouflageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Adjusted cooldown after activation
        }

        private void DeactivateCamouflage()
        {
            Hidden = false;
            m_CamouflageEnd = DateTime.MinValue;
        }

        private void LaunchRazorShards()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Launches razor-sharp quills! *");
            PlaySound(0x23B); // Whoosh sound
            FixedEffect(0x36BD, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    m.PlaySound(0x1E1);

                    Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 5, new TimerStateCallback(DealDamageOverTime), m);
                }
            }

            m_NextRazorShards = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Adjusted cooldown after launching
        }

        private void DealDamageOverTime(object state)
        {
            if (state is Mobile target && target.Alive)
            {
                target.Damage(Utility.RandomMinMax(3, 5), this);
            }
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }
}
