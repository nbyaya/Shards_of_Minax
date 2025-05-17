using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("the frostbound sovereign's remains")]
    public class FrostboundSovereign : BaseCreature
    {
        private DateTime m_NextFrozenNova;
        private DateTime m_NextMirrorShard;
        private DateTime m_NextBlizzardAvatar;
        private DateTime m_NextTemporalFreeze;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostboundSovereign()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "The Frostbound Sovereign";
            Body = 135; // Same body as Arctic Ogre Lord
            BaseSoundID = 427;
            Hue = 1152; // Pale glacial blue hue for otherworldly frost appearance

            SetStr(1100, 1300);
            SetDex(90, 120);
            SetInt(600, 750);

            SetHits(2000, 2500);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);

            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 75;
        }

        public FrostboundSovereign(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 8);

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new FrostboundWarhelm());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrozenNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                    m_NextMirrorShard = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextBlizzardAvatar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
                    m_NextTemporalFreeze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrozenNova)
                    CastFrozenNova();

                if (DateTime.UtcNow >= m_NextMirrorShard)
                    CastMirrorShard();

                if (DateTime.UtcNow >= m_NextBlizzardAvatar)
                    SummonBlizzardAvatar();

                if (DateTime.UtcNow >= m_NextTemporalFreeze)
                    CastTemporalFreeze();
            }
        }

        private void CastFrozenNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Sovereign releases a chilling nova! *");
            PlaySound(0x64C); // Frosty explosion sound
            Effects.SendLocationEffect(Location, Map, 0x10D3, 16, 1);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m is Mobile target)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(25, 45), 0, 100, 0, 0, 0);
                        target.Freeze(TimeSpan.FromSeconds(2.0));
                        target.SendMessage(0x480, "The Frostbound Sovereign's nova chills you to the bone!");
                    }
                }
            }

            m_NextFrozenNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
        }

        private void CastMirrorShard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* Shards of mirrored ice erupt from the Sovereign's body! *");
            PlaySound(0x5B3);
            Effects.SendLocationEffect(Location, Map, 0x374A, 16, 1);

            if (Combatant is Mobile target)
            {
                AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                target.SendMessage(0x480, "Mirror shards rip into your skin, reflecting your fears!");

                if (Utility.RandomDouble() < 0.3)
                {
                    target.Freeze(TimeSpan.FromSeconds(1.5));
                    target.PlaySound(0x10B);
                }
            }

            m_NextMirrorShard = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
        }

        private void SummonBlizzardAvatar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Sovereign calls upon a blizzard to fight in its stead! *");
            PlaySound(0x5C9);

            Mobile avatar = new BlizzardAvatar();
            avatar.MoveToWorld(Location, Map);
            avatar.Combatant = Combatant;

            m_NextBlizzardAvatar = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void CastTemporalFreeze()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "* Time slows... your limbs stiffen... *");
            PlaySound(0x5C7);

            if (Combatant is Mobile target)
            {
                target.Freeze(TimeSpan.FromSeconds(3.5));
                target.SendMessage(0x480, "You are frozen in time by the Sovereign’s power!");
            }

            m_NextTemporalFreeze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(45, 90));
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
            m_AbilitiesInitialized = false;
        }
    }

    public class BlizzardAvatar : BaseCreature
    {
        [Constructable]
        public BlizzardAvatar()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Blizzard Avatar";
            Body = 164;
            BaseSoundID = 0x56E;
            Hue = 1153;

            SetStr(300, 350);
            SetDex(100, 120);
            SetInt(50, 70);

            SetHits(350, 400);
            SetDamage(12, 18);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 30);
            SetResistance(ResistanceType.Cold, 100);

            SetSkill(SkillName.MagicResist, 60.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 75.0);

            ControlSlots = 0;
            Tamable = false;
        }

        public BlizzardAvatar(Serial serial) : base(serial) { }

        public override void GenerateLoot() { }

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
