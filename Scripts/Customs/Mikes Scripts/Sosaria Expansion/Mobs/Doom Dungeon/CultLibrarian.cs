using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class CultLibrarian : BaseCreature
    {
        private DateTime m_NextWhisper;

        [Constructable]
        public CultLibrarian() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("male") + " the Cult Librarian";
            Body = 0x190;
            Hue = 2301; // Sickly grey
            Title = "of the Doom Cult";

            AddItem(new Robe { Hue = 2118 });
            AddItem(new Sandals { Hue = 2406 });

            SetStr(600);
            SetDex(150);
            SetInt(500);

            SetHits(500);
            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Fire, 30);
            SetResistance(ResistanceType.Cold, 30);
            SetResistance(ResistanceType.Poison, 40);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;
            m_NextWhisper = DateTime.Now + TimeSpan.FromSeconds(8.0);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextWhisper)
            {
                Say(true, "*Whispers in an unknown tongue… the words burn your ears*");
                if (Combatant is Mobile target)
                {
                    target.FixedParticles(0x3709, 10, 30, 5044, 0, 0, EffectLayer.Head);
                    target.AddStatMod(new StatMod(StatType.Int, "LibrarianDebuff", -5, TimeSpan.FromSeconds(20)));
                    target.SendMessage(0x22, "Your mind feels foggy and unclean...");
                }

                m_NextWhisper = DateTime.Now + TimeSpan.FromSeconds(15.0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            PackItem(new BlasphemousGrimoire());
        }

        public CultLibrarian(Serial serial) : base(serial) { }

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
