using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a feast master")]
    public class FeastMaster : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between feast master speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public FeastMaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Feast Master";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Feast Master";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

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

            SetStr(300, 400);
            SetDex(200, 300);
            SetInt(400, 500);

            SetHits(500, 700);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 85.5, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

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
                        case 0: this.Say(true, "Come and feast!"); break;
                        case 1: this.Say(true, "Enjoy the meal!"); break;
                        case 2: this.Say(true, "Feast upon your demise!"); break;
                        case 3: this.Say(true, "Savor your last moments!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My feast... ruined..."); break;
                case 1: this.Say(true, "I... will return..."); break;
            }

            PackItem(new Apple(Utility.RandomMinMax(10, 20)));
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            if (from != null && from.Map == this.Map && from.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    TrapFeast(from);
                }
            }
        }

        private void TrapFeast(Mobile target)
        {
            this.Say(true, "Feast upon this!");

            TrapableContainer feast = new MetalChest();
            feast.TrapType = TrapType.ExplosionTrap;
            feast.TrapPower = Utility.RandomMinMax(30, 50);
            feast.TrapLevel = 3;
            feast.MoveToWorld(target.Location, target.Map);
        }

        public FeastMaster(Serial serial) : base(serial)
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
