using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a con artist")]
    public class ConArtist : BaseCreature
    {
        private TimeSpan m_TrickDelay = TimeSpan.FromSeconds(15.0); // time between tricks
        public DateTime m_NextTrickTime;

        [Constructable]
        public ConArtist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Con Artist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Con Artist";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item fancyShirt = new FancyShirt(Utility.RandomBrightHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(fancyShirt);
            AddItem(shoes);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 75.1, 100.0);
            SetSkill(SkillName.Magery, 80.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 60.5, 80.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            m_NextTrickTime = DateTime.Now + m_TrickDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTrickTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int trick = Utility.Random(2);

                    switch (trick)
                    {
                        case 0:
                            // Trick enemy into dropping an item
                            Item toSteal = combatant.Backpack.FindItemByType(typeof(Item));

                            if (toSteal != null)
                            {
                                this.Say(true, "Hand that over!");
                                combatant.Backpack.DropItem(toSteal);
                            }
                            break;

                        case 1:
                            // Trick enemy into changing position
                            this.Say(true, "Look over there!");
                            combatant.Location = new Point3D(combatant.X + Utility.RandomMinMax(-2, 2), combatant.Y + Utility.RandomMinMax(-2, 2), combatant.Z);
                            break;
                    }

                    m_NextTrickTime = DateTime.Now + m_TrickDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Average);

            this.Say(true, "You won't outsmart me...");

            PackItem(new Garlic(Utility.RandomMinMax(5, 10)));
        }

        public ConArtist(Serial serial) : base(serial)
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
