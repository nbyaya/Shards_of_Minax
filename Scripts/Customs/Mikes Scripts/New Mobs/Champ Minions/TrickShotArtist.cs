using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a trick shot artist")]
    public class TrickShotArtist : BaseCreature
    {
        private TimeSpan m_SpecialArrowDelay = TimeSpan.FromSeconds(20.0); // time between special arrow shots
        public DateTime m_NextSpecialArrowTime;

        [Constructable]
        public TrickShotArtist() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Trick Shot Artist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Trick Shot Artist";
            }

            Item hat = new FeatheredHat(Utility.RandomNeutralHue());
            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());

            AddItem(hat);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            Item bow = new Bow();
            AddItem(bow);
            bow.Movable = false;

            SetStr(700, 900);
            SetDex(200, 300);
            SetInt(150, 250);

            SetHits(500, 800);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Archery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 85.1, 100.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            m_NextSpecialArrowTime = DateTime.Now + m_SpecialArrowDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpecialArrowTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    if (Utility.RandomBool())
                    {
                        FireNetArrow(combatant);
                    }
                    else
                    {
                        FireSmokeArrow(combatant);
                    }

                    m_NextSpecialArrowTime = DateTime.Now + m_SpecialArrowDelay;
                }
            }

            base.OnThink();
        }

        public void FireNetArrow(Mobile target)
        {
            this.Say(true, "Try dodging this!");
            // Implement the net arrow effect here
            // Example: target.Paralyze(TimeSpan.FromSeconds(5.0));
        }

        public void FireSmokeArrow(Mobile target)
        {
            this.Say(true, "Disappear in smoke!");
            // Implement the smoke arrow effect here
            // Example: target.AddStatMod(new StatMod(StatType.Dex, "SmokeArrow", -20, TimeSpan.FromSeconds(10.0)));
        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
            AddLoot(LootPack.Rich);
        }

        public TrickShotArtist(Serial serial) : base(serial)
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
