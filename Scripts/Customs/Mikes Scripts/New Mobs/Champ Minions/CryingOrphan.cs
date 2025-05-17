using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a crying orphan")]
    public class CryingOrphan : BaseCreature
    {
        private TimeSpan m_CryDelay = TimeSpan.FromSeconds(10.0); // time between cries
        public DateTime m_NextCryTime;

        [Constructable]
        public CryingOrphan() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Crying Orphan";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Crying Orphan";
            }

            Item clothes = new Robe();
            AddItem(clothes);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(100, 150);
            SetDex(50, 70);
            SetInt(80, 100);

            SetHits(60, 100);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 20.1, 40.0);
            SetSkill(SkillName.MagicResist, 50.0, 75.0);
            SetSkill(SkillName.Tactics, 40.1, 60.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;

            m_NextCryTime = DateTime.Now + m_CryDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextCryTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Why did you leave me?"); break;
                        case 1: this.Say(true, "I just want my mommy!"); break;
                        case 2: this.Say(true, "I'm so alone..."); break;
                    }

                    // Apply debuff to combatant
                    combatant.SendMessage("You are overcome with sorrow.");
                    combatant.Dex -= 5; // Reducing dexterity temporarily
                    combatant.Stam -= 10; // Reducing stamina temporarily
                    
                    m_NextCryTime = DateTime.Now + m_CryDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(50, 100);
            AddLoot(LootPack.Poor);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "No one cares about me..."); break;
                case 1: this.Say(true, "I'm just a burden..."); break;
            }
        }

        public CryingOrphan(Serial serial) : base(serial)
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
