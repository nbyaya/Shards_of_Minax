using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a diviner")]
    public class Diviner : BaseCreature
    {
        private TimeSpan m_CardDrawDelay = TimeSpan.FromSeconds(10.0); // time between card draws
        public DateTime m_NextCardDrawTime;

        [Constructable]
        public Diviner() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Diviner";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Diviner";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item sandals = new Sandals(Utility.RandomNeutralHue());
            AddItem(robe);
            AddItem(sandals);

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

            SetStr(500, 700);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 85.5, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextCardDrawTime = DateTime.Now + m_CardDrawDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextCardDrawTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    DrawTarotCard(combatant);
                    m_NextCardDrawTime = DateTime.Now + m_CardDrawDelay;
                }

                base.OnThink();
            }
        }

        private void DrawTarotCard(Mobile target)
        {
            int card = Utility.Random(4);

            switch (card)
            {
                case 0:
                    this.Say(true, "The Fool: You are blessed with luck!");
                    Buff(target);
                    break;
                case 1:
                    this.Say(true, "The Magician: Your power is diminished.");
                    Debuff(target);
                    break;
                case 2:
                    this.Say(true, "The High Priestess: Knowledge flows through you.");
                    Buff(target);
                    break;
                case 3:
                    this.Say(true, "The Tower: Misfortune befalls you.");
                    Debuff(target);
                    break;
            }
        }

        private void Buff(Mobile target)
        {
            // Example buff effect
            target.Hits += 20; // Increase target's health
            target.SendMessage("You feel a surge of energy!");
        }

        private void Debuff(Mobile target)
        {
            // Example debuff effect
            target.Hits -= 20; // Decrease target's health
            target.SendMessage("You feel weakened!");
        }

        public Diviner(Serial serial) : base(serial)
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
