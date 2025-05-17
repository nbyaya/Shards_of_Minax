using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an icy executioner corpse")]
    public class IcyExecutioner : BaseCreature
    {
        private DateTime m_NextFrozenStrike;
        private DateTime m_NextSoulChill;
        private DateTime m_NextShatterHowl;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IcyExecutioner()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icy Executioner";
            Body = 0x190; // Same body as base Executioner
            Hue = 1152;   // Pale ice-blue hue
            BaseSoundID = 0x1F1;

            SetStr(800, 950);
            SetDex(180, 220);
            SetInt(220, 280);

            SetHits(1500, 1800);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Swords, 110.0, 125.0);
            SetSkill(SkillName.Tactics, 110.0, 125.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.SpiritSpeak, 90.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 75;

            AddItem(new HoodedShroudOfShadows { Hue = 1152 });
            AddItem(new ExecutionersAxe { Hue = 1150 });

            Utility.AssignRandomHair(this);

            m_AbilitiesInitialized = false;
        }

        public IcyExecutioner(Serial serial) : base(serial) { }

        public override bool AlwaysMurderer => true;
        public override bool CanRummageCorpses => true;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.005)
                PackItem(new IceboundCrown()); // Rare artifact drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_AbilitiesInitialized)
            {
                var rand = new Random();
                m_NextFrozenStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(3, 10));
                m_NextSoulChill = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                m_NextShatterHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextFrozenStrike)
                FrozenStrike();

            if (DateTime.UtcNow >= m_NextSoulChill)
                SoulChill();

            if (DateTime.UtcNow >= m_NextShatterHowl)
                ShatterHowl();
        }

        private void FrozenStrike()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, false, "*delivers a frostbitten cleave!*");
                PlaySound(0x5C9); // Cold strike sound
                target.Freeze(TimeSpan.FromSeconds(2));
                AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 0, 0, 100, 0, 0);
                target.SendMessage(0x480, "You are struck by freezing force!");

                m_NextFrozenStrike = DateTime.UtcNow + TimeSpan.FromSeconds(8);
            }
        }

        private void SoulChill()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, false, "*lets out a chilling curse...*");
                PlaySound(0x64C); // Curse sound

                Effects.SendLocationEffect(Location, Map, 0x375A, 20, 10, 1150, 0);
                target.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Head);
                target.PlaySound(0x1ED);
                if (Utility.RandomDouble() < 0.15)
                    target.ApplyPoison(this, Poison.Regular);

                m_NextSoulChill = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private void ShatterHowl()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*releases a bone-shattering howl!*");
            PlaySound(0x10B); // Roar
            Effects.PlaySound(Location, Map, 0x10B);
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && m is Mobile target)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(20, 40), 0, 0, 100, 0, 0);
                    target.SendMessage(0x480, "The freezing howl rattles your soul!");
                    target.Freeze(TimeSpan.FromSeconds(1.5));
                }
            }

            m_NextShatterHowl = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override int Damage(int amount, Mobile from, bool informMount, bool checkDisrupt)
        {
            int dam = base.Damage(amount, from, informMount, checkDisrupt);

            if (from != null && dam > 0 && Utility.RandomDouble() < 0.25)
            {
                from.SendMessage(0x480, "Frost creeps up your limbs, slowing you...");
                from.Freeze(TimeSpan.FromSeconds(1));
            }

            return dam;
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
    }

    public class IceboundCrown : BaseHat
    {
        [Constructable]
        public IceboundCrown() : base(0x1715) // Circlet
        {
            Name = "the Icebound Crown";
            Hue = 1152;
            LootType = LootType.Blessed;
            Attributes.BonusInt = 5;
            Attributes.CastRecovery = 2;
            Resistances.Cold = 25;
        }

        public IceboundCrown(Serial serial) : base(serial) { }

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
