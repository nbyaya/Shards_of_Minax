using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a tree feller")]
    public class TreeFeller : BaseCreature
    {
        private TimeSpan m_TreeFallDelay = TimeSpan.FromSeconds(30.0); // time between tree falls
        public DateTime m_NextTreeFallTime;

        [Constructable]
        public TreeFeller() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Tree Feller";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Tree Feller";
            }

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

            Item plaidShirt = new Shirt();
            plaidShirt.Hue = 0x66D;
            AddItem(plaidShirt);

            Item boots = new Boots();
            boots.Hue = Utility.RandomNeutralHue();
            AddItem(boots);

            Item axe = new ExecutionersAxe();
            axe.Movable = false;
            AddItem(axe);

            SetStr(900, 1300);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(700, 1200);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Lumberjacking, 100.0, 120.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 60;

            m_NextTreeFallTime = DateTime.Now + m_TreeFallDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTreeFallTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    this.Say(true, "Feel the wrath of the forest!");
                    combatant.Damage(Utility.RandomMinMax(50, 100), this);
                    combatant.SendMessage("A tree falls on you, causing massive damage!");

                    m_NextTreeFallTime = DateTime.Now + m_TreeFallDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The forest... it calls me..."); break;
                case 1: this.Say(true, "You will pay for this..."); break;
            }

            PackItem(new Log(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You think you can stop me?!"); break;
                        case 1: this.Say(true, "Is that all you got?"); break;
                        case 2: this.Say(true, "I've faced worse than you!"); break;
                        case 3: this.Say(true, "You're no match for me!"); break;
                    }
                }
            }

            return base.Damage(amount, from);
        }

        public TreeFeller(Serial serial) : base(serial)
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
