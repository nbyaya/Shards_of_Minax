using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a pocket picker")]
    public class PocketPicker : BaseCreature
    {
        private TimeSpan m_StealDelay = TimeSpan.FromSeconds(30.0); // time between steal attempts
        public DateTime m_NextStealTime;

        [Constructable]
        public PocketPicker() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = "the Pocket Picker";
			Team = 2;

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt();
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            SetStr(300, 500);
            SetDex(500, 700);
            SetInt(100, 200);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Stealing, 90.1, 100.0);
            SetSkill(SkillName.Hiding, 80.1, 90.0);
            SetSkill(SkillName.Stealth, 90.1, 100.0);
            SetSkill(SkillName.Fencing, 70.1, 90.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 30;

            m_NextStealTime = DateTime.Now + m_StealDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextStealTime)
            {
                TrySteal();
                m_NextStealTime = DateTime.Now + m_StealDelay;
            }
        }

        private void TrySteal()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 1))
            {
                Container pack = combatant.Backpack;

                if (pack != null)
                {
                    Item item = pack.FindItemByType(typeof(Gold));

                    if (item != null)
                    {
                        int amount = Math.Min(item.Amount, Utility.RandomMinMax(10, 50));

                        if (amount > 0)
                        {
                            item.Amount -= amount;

                            if (item.Amount <= 0)
                                item.Delete();

                            this.AddToBackpack(new Gold(amount));
                            this.Say(true, "Your pockets are lighter now!");
                        }
                    }
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(100, 200);
            AddLoot(LootPack.Average);
        }

        public PocketPicker(Serial serial) : base(serial)
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
