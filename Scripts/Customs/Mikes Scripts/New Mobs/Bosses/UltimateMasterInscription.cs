using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Johannes Gutenberg")]
    public class UltimateMasterInscription : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterInscription()
            : base(AIType.AI_Mage)
        {
            Name = "Johannes Gutenberg";
            Title = "The Master of Inscription";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Inscribe, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());

            HairItemID = 0x2044; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterInscription(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PrintersInk), typeof(GutenbergsPress) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ScribesPen), typeof(EnchantedParchment) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ScribesPen), typeof(EnchantedParchment) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Inscribe, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PrintersInk());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new GutenbergsPress());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Scrollstorm(); break;
                    case 1: GlyphOfProtection(); break;
                    case 2: ScriptedStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Scrollstorm()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(60, 80);

                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
            }
        }

        public void GlyphOfProtection()
        {
            this.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
            this.PlaySound(0x1F7);

            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(RemoveProtection));
        }

        public void RemoveProtection()
        {
            this.VirtualArmor -= 20;
        }

        public void ScriptedStrike(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x3779, 1, 15, 0x26EC, 0x3F, 0, EffectLayer.Waist);
                defender.PlaySound(0x1E1);

                AOS.Damage(defender, this, Utility.RandomMinMax(40, 60), 0, 100, 0, 0, 0);

                if (Utility.RandomDouble() < 0.5)
                {
                    defender.SendMessage("You have been silenced!");
                }
            }
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
