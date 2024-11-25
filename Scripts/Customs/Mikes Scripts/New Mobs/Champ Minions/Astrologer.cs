using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an astrologer")]
    public class Astrologer : BaseCreature
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(15.0); // time between spells
        public DateTime m_NextSpellTime;

        [Constructable]
        public Astrologer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Male body
            Name = NameList.RandomName("male");
            Title = "the Astrologer";
			Team = 2;

            // Equipment
            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item staff = new QuarterStaff();
            AddItem(staff);
            staff.Movable = false;

            SetStr(200, 300);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpellTime = DateTime.Now + m_SpellDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpellTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    CastCelestialSpell(combatant);
                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }

                base.OnThink();
            }
        }

        private void CastCelestialSpell(Mobile target)
        {
            switch (Utility.Random(4))
            {
                case 0: // Meteor Shower
                    this.Say(true, "The stars rain down upon you!");
                    target.Damage(Utility.RandomMinMax(20, 40), this);
                    break;
                case 1: // Solar Flare
                    this.Say(true, "Feel the heat of the sun!");
                    target.Damage(Utility.RandomMinMax(30, 50), this);
                    target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5)));
                    break;
                case 2: // Lunar Eclipse
                    this.Say(true, "The moon's shadow consumes you!");
                    target.Damage(Utility.RandomMinMax(10, 30), this);
                    target.Mana -= Utility.RandomMinMax(10, 20);
                    break;
                case 3: // Cosmic Burst
                    this.Say(true, "The cosmos explodes in your face!");
                    target.Damage(Utility.RandomMinMax(25, 45), this);
                    break;
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 300);
            AddLoot(LootPack.Rich);

            PackItem(new StarSapphire(Utility.RandomMinMax(1, 3)));
        }

        public Astrologer(Serial serial) : base(serial)
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
