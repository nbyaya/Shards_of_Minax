using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Morgan le Fay")]
    public class UltimateMasterSpellweaver : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterSpellweaver()
            : base(AIType.AI_Mage)
        {
            Name = "Morgan le Fay";
            Title = "The Enchantress";
            Body = 0x191;
            Hue = 0x83F;

            SetStr(280, 400);
            SetDex(75, 150);
            SetInt(600, 800);

            SetHits(10000);
            SetMana(3000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Spellweaving, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;
			
			AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterSpellweaver(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(EnchantedStaff), typeof(DarkGrimoire) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(EnchantedStaff), typeof(DarkGrimoire) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(EnchantedStaff), typeof(DarkGrimoire) }; }
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

            c.DropItem(new PowerScroll(SkillName.Spellweaving, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new EnchantedStaff());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DarkGrimoire());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: ArcaneBurst(); break;
                    case 1: EnchantArmor(); break;
                    case 2: SummonFamiliar(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ArcaneBurst()
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

                int damage = Utility.RandomMinMax(80, 100);

                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                m.FixedParticles(0x374A, 10, 30, 5029, EffectLayer.Head);
                m.PlaySound(0x213);
            }
        }

        public void EnchantArmor()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                m.PlaySound(0x1EA);

                m.VirtualArmorMod += 30; // Increase defense by 30

                Timer.DelayCall(TimeSpan.FromSeconds(10), delegate { m.VirtualArmorMod -= 30; }); // Revert defense after 10 seconds
            }
        }

        public void SummonFamiliar()
        {
            BaseCreature familiar = new Familiar();
            familiar.Team = this.Team;
            familiar.Map = this.Map;
            familiar.Location = this.Location;
            familiar.Combatant = this.Combatant;

            familiar.FixedParticles(0x3728, 1, 13, 9911, 95, 2, EffectLayer.Waist);
            familiar.PlaySound(0x204);
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
