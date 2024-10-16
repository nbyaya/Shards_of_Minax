using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a nerve agent")]
    public class NerveAgent : BaseCreature
    {
        private TimeSpan m_StunDelay = TimeSpan.FromSeconds(20.0); // time between stun attempts
        public DateTime m_NextStunTime;

        [Constructable]
        public NerveAgent() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Nerve Agent";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Nerve Agent";
            }

            AddItem(new Robe(Utility.RandomBrightHue()));  // Brightly colored robe to signify danger

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item weapon = new Katana(); // Electrical attacks represented by a wand
            weapon.Movable = false;
            AddItem(weapon);

            SetStr(900, 1000);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(700, 900);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Energy, 100); // Pure energy damage

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 55, 70);

            SetSkill(SkillName.EvalInt, 100.1, 110.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.Meditation, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Tactics, 50.1, 65.0);
            SetSkill(SkillName.Wrestling, 50.1, 65.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 50;

            m_NextStunTime = DateTime.Now + m_StunDelay;
        }

        public override void OnThink()
        {
            base.OnThink();
            if (DateTime.Now >= m_NextStunTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    combatant.Freeze(m_StunDelay); // Apply stun effect
                    this.Say("Prepare to be disoriented!"); // Announce attack
                    m_NextStunTime = DateTime.Now + m_StunDelay;
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);
            AddLoot(LootPack.UltraRich);
        }

        public NerveAgent(Serial serial) : base(serial)
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
