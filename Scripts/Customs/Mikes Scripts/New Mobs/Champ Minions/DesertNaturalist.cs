using System;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a desert naturalist")]
    public class DesertNaturalist : BaseCreature
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(10.0); // time between spells
        public DateTime m_NextSpellTime;

        [Constructable]
        public DesertNaturalist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Title = "the Naturalist";

            Body = Utility.RandomBool() ? 0x191 : 0x190; // Female or Male
            Name = NameList.RandomName(Body == 0x191 ? "female" : "male");

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(400, 600);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 110.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 75.0, 95.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 4000;
            Karma = 4000;

            VirtualArmor = 40;

            m_NextSpellTime = DateTime.Now + m_SpellDelay;

            PackItem(new Sandals(Utility.RandomNeutralHue()));
            PackItem(new Robe(Utility.RandomNeutralHue()));

            if (!this.Female)
            {
                PackItem(new ShortPants(Utility.RandomNeutralHue()));
            }
            else
            {
                PackItem(new Kilt(Utility.RandomNeutralHue()));
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            if (DateTime.Now >= m_NextSpellTime && Combatant != null)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 12))
                {
                    // Summon a desert creature
                    switch (Utility.Random(2))
                    {
                        case 0:
                            Summon(new Scorpion(), target);
                            break;
                        case 1:
                            Summon(new Snake(), target);
                            break;
                    }

                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }
            }
        }

        private void Summon(BaseCreature creature, Mobile target)
        {
            Map map = this.Map;
            if (map != null)
            {
                Point3D location = new Point3D(target.X + 2, target.Y + 2, target.Z);
                creature.MoveToWorld(location, map);
                creature.Combatant = target;
                this.Say(true, "Arise, my creature, and serve me!");
            }
        }

        public DesertNaturalist(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
