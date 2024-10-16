using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an anvil hurler")]
    public class AnvilHurler : BaseCreature
    {
        private TimeSpan m_ThrowDelay = TimeSpan.FromSeconds(5.0); // time between anvil throws
        public DateTime m_NextThrowTime;

        [Constructable]
        public AnvilHurler() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Anvil Hurler";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Anvil Hurler";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(pants);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(900, 1200);
            SetDex(100, 150);
            SetInt(50, 70);

            SetHits(700, 900);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Throwing, 90.0, 120.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

            m_NextThrowTime = DateTime.Now + m_ThrowDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextThrowTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    ThrowAnvil(combatant);
                    m_NextThrowTime = DateTime.Now + m_ThrowDelay;
                }

                base.OnThink();
            }
        }

        private void ThrowAnvil(Mobile target)
        {
            this.Say("Feel the weight of my anvil!");
            this.MovingEffect(target, 0x2D26, 10, 1, false, false);
            AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);
            AddLoot(LootPack.Rich);

            this.Say("You won't take my anvils...");
            PackItem(new IronIngot(Utility.RandomMinMax(10, 20)));
        }

        public AnvilHurler(Serial serial) : base(serial)
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
