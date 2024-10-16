using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a rune keeper")]
    public class RuneKeeper : BaseCreature
    {
        private TimeSpan m_RunePlacementDelay = TimeSpan.FromSeconds(30.0); // time between rune placements
        public DateTime m_NextRunePlacementTime;

        [Constructable]
        public RuneKeeper() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Rune Keeper";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Rune Keeper";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item boots = new Sandals(Utility.RandomNeutralHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(boots);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(500, 700);

            SetHits(500, 700);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

            m_NextRunePlacementTime = DateTime.Now + m_RunePlacementDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextRunePlacementTime)
            {
                PlaceRune();
                m_NextRunePlacementTime = DateTime.Now + m_RunePlacementDelay;
            }

            base.OnThink();
        }

        private void PlaceRune()
        {
            if (Combatant != null && Combatant.Map == this.Map)
            {
                if (Utility.RandomBool())
                {
                    this.Say("Runes of power, heed my call!");
                    // Boost allies' resistances in range
                    foreach (Mobile ally in this.GetMobilesInRange(5))
                    {
                        if (ally != this && ally is BaseCreature && ((BaseCreature)ally).ControlMaster == this.ControlMaster)
                        {
                            ally.SendMessage("You feel empowered by the rune!");
                            // Apply buff to ally
                            ally.VirtualArmorMod += 20;
                        }
                    }
                }
                else
                {
                    this.Say("Runes of weakening, cripple my foes!");
                    // Weaken enemies' resistances in range
                    foreach (Mobile enemy in this.GetMobilesInRange(5))
                    {
                        if (enemy != this && enemy is BaseCreature && ((BaseCreature)enemy).ControlMaster != this.ControlMaster)
                        {
                            enemy.SendMessage("You feel weakened by the rune!");
                            // Apply debuff to enemy
                            enemy.VirtualArmorMod -= 20;
                        }
                    }
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);
        }

        public RuneKeeper(Serial serial) : base(serial)
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
