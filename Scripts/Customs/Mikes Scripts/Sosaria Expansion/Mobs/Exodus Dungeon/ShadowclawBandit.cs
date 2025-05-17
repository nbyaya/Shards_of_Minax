using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a shadowclaw bandit corpse")]
    public class ShadowclawBandit : BaseCreature
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextPoisonMist;
        private DateTime m_NextMirrorStep;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ShadowclawBandit()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Shadowclaw Bandit";
            Title = "of the Umbral Veil";
            Female = Utility.RandomBool();
            Race = Race.Human;
            Hue = 1175; // Unique dark purple/black hue
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            Body = 0x190;
            BaseSoundID = 0x1A3; // Same sound as TigersClawThief

            AddItem(new LeatherNinjaPants { Hue = 1109 });
            AddItem(new StuddedMempo { Hue = 1109 });
            AddItem(new JinBaori { Hue = 1175 });
            AddItem(new Wakizashi());
            AddItem(new LightPlateJingasa { Hue = 1175 });
            AddItem(new StuddedGloves { Hue = 1175 });

            SetStr(600, 650);
            SetDex(450, 500);
            SetInt(300, 350);

            SetHits(1500, 1600);

            SetDamage(22, 28);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 45, 60);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 110.0, 125.0);
            SetSkill(SkillName.Stealth, 120.0, 140.0);
            SetSkill(SkillName.Hiding, 120.0, 140.0);
            SetSkill(SkillName.Ninjitsu, 120.0, 140.0);
            SetSkill(SkillName.Fencing, 115.0, 130.0);

            Fame = 22000;
            Karma = -22000;
            VirtualArmor = 75;

            m_AbilitiesInitialized = false;
        }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.01) // 1% chance
                PackItem(new UmbralVeilToken()); // Custom rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextPoisonMist = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                    m_NextMirrorStep = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                    ShadowStrike();

                if (DateTime.UtcNow >= m_NextPoisonMist)
                    PoisonMist();

                if (DateTime.UtcNow >= m_NextMirrorStep)
                    MirrorStep();
            }
        }

        private void ShadowStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* vanishes briefly in a blur of shadows *");
            Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                Hidden = false;
                PlaySound(0x1F1); // Sneak attack sound
                if (Combatant != null && Combatant is Mobile target)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
                    target.SendMessage(0x22, "A shadow blade cuts deep into your side!");
                }
            });

            m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void PoisonMist()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* exhales a cloud of noxious mist *");
            PlaySound(0x231); // Poison cloud

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m is Mobile target)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                        target.ApplyPoison(this, Poison.Greater);
                        target.SendMessage(0x44, "The toxic mist burns your lungs!");
                    }
                }
            }

            m_NextPoisonMist = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MirrorStep()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* splits into multiple mirror images *");
            PlaySound(0x20A); // Illusion

            // Illusory clone effect (purely visual or RP-based decoys if desired)
            Effects.SendLocationEffect(Location, Map, 0x3728, 30, 10);

            if (Combatant is Mobile target)
            {
                target.SendMessage(0x3C, "You struggle to distinguish the real bandit from illusions!");
                target.RevealingAction();
                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    if (target.Alive)
                        AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 50, 0, 50);
                });
            }

            m_NextMirrorStep = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.15)
                c.DropItem(new ShadowVeilCloak()); // Custom loot item
        }

        public ShadowclawBandit(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    // Example rare drop
    public class UmbralVeilToken : Item
    {
        public UmbralVeilToken() : base(0x2AAA)
        {
            Name = "a token of the Umbral Veil";
            Hue = 1175;
            LootType = LootType.Blessed;
        }

        public UmbralVeilToken(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ShadowVeilCloak : Cloak
    {
        [Constructable]
        public ShadowVeilCloak() : base()
        {
            Name = "Shadow Veil Cloak";
            Hue = 1175;
            LootType = LootType.Regular;
        }

        public ShadowVeilCloak(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
