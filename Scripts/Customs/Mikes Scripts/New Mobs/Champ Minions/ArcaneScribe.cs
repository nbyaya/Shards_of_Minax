using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an arcane scribe")]
    public class ArcaneScribe : BaseCreature
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(15.0); // time between spells
        public DateTime m_NextSpellTime;

        [Constructable]
        public ArcaneScribe() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Arcane Scribe";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Arcane Scribe";
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

            SetStr(400, 500);
            SetDex(100, 150);
            SetInt(600, 800);

            SetHits(300, 400);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 50.0, 60.0);
            SetSkill(SkillName.Wrestling, 20.1, 30.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpellTime = DateTime.Now + m_SpellDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpellTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    int spellChoice = Utility.Random(3);

                    switch (spellChoice)
                    {
                        case 0:
                            this.Say(true, "Feel the burn of my flames!");
                            this.DoHarmful(combatant);
                            Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.5), combatant, this, Utility.RandomMinMax(20, 30));
                            break;
                        case 1:
                            this.Say(true, "Freeze in your tracks!");
                            this.DoHarmful(combatant);
                            Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.5), combatant, this, Utility.RandomMinMax(15, 25));
                            break;
                        case 2:
                            this.Say(true, "Be shocked by my power!");
                            this.DoHarmful(combatant);
                            Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.5), combatant, this, Utility.RandomMinMax(25, 35));
                            break;
                    }

                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
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
                case 0: this.Say(true, "My spells... they fail me..."); break;
                case 1: this.Say(true, "I... am... undone..."); break;
            }

            PackItem(new Spellbook());
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Your efforts are futile!"); break;
                        case 1: this.Say(true, "I will not be so easily defeated!"); break;
                        case 2: this.Say(true, "My magic is stronger than you!"); break;
                    }

                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public ArcaneScribe(Serial serial) : base(serial)
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
