using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a fossil elemental corpse")]
    public class FossilElemental : BaseCreature
    {
        private DateTime m_NextBoneArmor;
        private DateTime m_NextFossilBurst;
        private bool m_BoneArmorActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FossilElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fossil elemental";
            Body = 14;
            BaseSoundID = 268;
            Hue = 1497; // Pale brown hue

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

            PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));
            PackItem(new Bone());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FossilElemental(Serial serial)
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

        public override bool BleedImmune { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBoneArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFossilBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBoneArmor)
                {
                    ActivateBoneArmor();
                }

                if (DateTime.UtcNow >= m_NextFossilBurst)
                {
                    FossilBurst();
                }
            }
        }

        private void ActivateBoneArmor()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Hardens its fossil armor *");
            PlaySound(0x65A);
            FixedEffect(0x37C4, 10, 10);

            m_BoneArmorActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                m_BoneArmorActive = false;
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The fossil armor crumbles *");
            });

            m_NextBoneArmor = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown to fixed interval
        }

        private void FossilBurst()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes ancient energy *");
            PlaySound(0x665);
            FixedEffect(0x36BD, 20, 10);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(5);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                m.SendLocalizedMessage(1070844); // The creature's aura of energy decreases your resistance to physical attacks.

                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -20);
                m.AddResistanceMod(mod);

                Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
                {
                    m.RemoveResistanceMod(mod);
                    m.SendLocalizedMessage(1070845); // Your resistance to physical attacks has returned.
                });
            }

            m_NextFossilBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown to fixed interval
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            base.AlterMeleeDamageFrom(from, ref damage);

            if (m_BoneArmorActive)
            {
                damage = (int)(damage * 0.6); // 40% damage reduction
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
        }
    }
}
