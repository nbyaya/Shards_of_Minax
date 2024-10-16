using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a pickpocket")]
    public class Pickpocket : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Pickpocket() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Pickpocket";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Pickpocket";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new FancyShirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            SetStr(200, 300);
            SetDex(300, 400);
            SetInt(100, 200);

            SetHits(250, 350);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Stealing, 90.1, 100.0);
            SetSkill(SkillName.Snooping, 90.1, 100.0);
            SetSkill(SkillName.Hiding, 90.1, 100.0);
            SetSkill(SkillName.Stealth, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.Fencing, 75.1, 100.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Your valuables are mine!"); break;
                        case 1: this.Say(true, "I'll be taking that!"); break;
                        case 2: this.Say(true, "Catch me if you can!"); break;
                        case 3: this.Say(true, "You won't even notice it's gone!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(50, 100);
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
            AddLoot(LootPack.Meager);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && from.Player && Utility.RandomDouble() < 0.25)
            {
                Item stolen = StealItem(from);

                if (stolen != null)
                {
                    this.Say(true, "I've taken your item");
                }
            }
        }

        public Item StealItem(Mobile from)
        {
            Item toSteal = from.Backpack.FindItemByType(typeof(Item));

            if (toSteal != null)
            {
                from.Backpack.RemoveItem(toSteal);
                this.AddItem(toSteal);

                return toSteal;
            }

            return null;
        }

        public Pickpocket(Serial serial) : base(serial)
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
