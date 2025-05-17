using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shadow lurker")]
    public class ShadowLurker : BaseCreature
    {
        private TimeSpan m_StealthDelay = TimeSpan.FromSeconds(10.0); // time between stealth actions
        public DateTime m_NextStealthTime;

        [Constructable]
        public ShadowLurker() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomNeutralHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Shadow Lurker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Shadow Lurker";
            }

            Item hood = new HoodedShroudOfShadows();
            Item boots = new Boots(Utility.RandomNeutralHue());
            hood.Hue = Utility.RandomNeutralHue();
            boots.Hue = Utility.RandomNeutralHue();

            AddItem(hood);
            AddItem(boots);

            SetStr(400, 600);
            SetDex(400, 600);
            SetInt(100, 200);

            SetHits(300, 500);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Stealing, 90.1, 100.0);
            SetSkill(SkillName.Hiding, 90.1, 100.0);
            SetSkill(SkillName.Stealth, 90.1, 100.0);
            SetSkill(SkillName.Fencing, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;

            m_NextStealthTime = DateTime.Now + m_StealthDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextStealthTime)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 8))
                {
                    // Shadow Lurker uses stealth or sabotage actions
                    int action = Utility.Random(2);

                    switch (action)
                    {
                        case 0: this.Say(true, "You can't see me..."); Stealth(); break;
                        case 1: this.Say(true, "Your items are mine!"); StealItem(target); break;
                    }

                    m_NextStealthTime = DateTime.Now + m_StealthDelay;
                }

                base.OnThink();
            }
        }

        private void Stealth()
        {
            // Implement stealth logic here
        }

        private void StealItem(Mobile target)
        {
            if (target.Backpack != null)
            {
                Item item = target.Backpack.FindItemByType(typeof(Gold));

                if (item != null)
                {
                    target.Backpack.RemoveItem(item);
                    this.AddToBackpack(item);
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            AddLoot(LootPack.Meager);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My shadow fades..."); break;
                case 1: this.Say(true, "You can't kill the darkness..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
        }

        public ShadowLurker(Serial serial) : base(serial)
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
