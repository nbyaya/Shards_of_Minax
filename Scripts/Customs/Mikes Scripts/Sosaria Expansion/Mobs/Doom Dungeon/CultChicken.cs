using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // For spell helpers/effects
using Server.Spells.Seventh; // In case we borrow effects

namespace Server.Mobiles
{
    [CorpseName("a cult chicken corpse")]
    public class CultChicken : BaseCreature
    {
        private DateTime m_NextPeckTime;
        private DateTime m_NextEggBombTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // A sinister crimson hue
        private const int UniqueHue = 2308;

        [Constructable]
        public CultChicken()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a cult chicken";
            Body = 0xD0;            // same as base chicken
            BaseSoundID = 0x6E;     // clucking
            Hue = UniqueHue;

            // —— Stats ——
            SetStr(350, 400);
            SetDex(150, 200);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(300, 400);

            SetDamage(20, 30);

            // —— Damage Types ——
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   20, 30);

            // —— Skills ——
            SetSkill(SkillName.Wrestling,    90.0, 100.0);
            SetSkill(SkillName.Tactics,      90.0, 100.0);
            SetSkill(SkillName.MagicResist,  85.0,  95.0);
            SetSkill(SkillName.Poisoning,   110.0, 120.0);
            SetSkill(SkillName.Anatomy,     100.0, 110.0);

            Fame   = 12500;
            Karma  = -12500;
            VirtualArmor = 70;
            ControlSlots  = 4;

            // —— Ability cooldowns ——
            m_NextPeckTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextEggBombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation    = this.Location;

            // —— Base loot ——
            PackItem(new ChickenLeg());   // Always drops some legs
            PackItem(new Feather(Utility.RandomMinMax(10, 20)));
        }

        // —— Apply toxic aura when players move nearby ——
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && m.Map == this.Map && Alive && m.Alive && m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    target.ApplyPoison(this, Poison.Lethal);
                    target.SendMessage(0x22, "You feel a deadly toxin course through your veins!");
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextPeckTime && this.InRange(Combatant.Location, 3))
            {
                DoPeckFrenzy();
                m_NextPeckTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
            else if (DateTime.UtcNow >= m_NextEggBombTime && this.InRange(Combatant.Location, 12))
            {
                DoEggBomb();
                m_NextEggBombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            else if (DateTime.UtcNow >= m_NextSummonTime)
            {
                DoSummonAcolytes();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        // —— Rapid AoE melee strikes ——
        private void DoPeckFrenzy()
        {
            if (!(Combatant is Mobile primary) || !CanBeHarmful(primary, false))
                return;

            this.Say("*The cult chicken goes into a frenzied peck!*");
            PlaySound(0x213);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 3))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int hits = Utility.RandomMinMax(2, 4);
                for (int i = 0; i < hits; i++)
                {
                    int dmg = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);
                }
                target.FixedParticles(0x3779, 5, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                target.SendMessage("The cult chicken delivers a brutal series of pecks!");
            }
        }

        // —— Ranged AoE poison egg projectiles ——
        private void DoEggBomb()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*The cult chicken flings explosive venomous eggs!*");
            PlaySound(0x227);

            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                double angle = Utility.RandomDouble() * Math.PI * 2;
                int radius = Utility.RandomMinMax(2, 5);
                Point3D loc = new Point3D(
                    target.X + (int)(Math.Cos(angle) * radius),
                    target.Y + (int)(Math.Sin(angle) * radius),
                    target.Z
                );

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        continue;
                }

                // Use PoisonTile as an egg that hatches into poison
                PoisonTile tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                    0x3728, 8, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        // —— Summon lesser cult chickens to aid ——
        private void DoSummonAcolytes()
        {
            this.Say("*The cult chicken summons its acolytes through dark ritual!*");
            PlaySound(0x22F);

            int num = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < num; i++)
            {
                Chicken acolyte = new Chicken();
                acolyte.Hue = UniqueHue;
                acolyte.Name = "a cult chicken acolyte";
                acolyte.Team = this.Team;
                acolyte.MoveToWorld(new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z
                ), this.Map);
                acolyte.Combatant = this.Combatant;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;
			
			this.Say("*The cult chicken lets out a final, unholy caw...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 40, UniqueHue, 0, 5052, 0
            );

            int hazards = Utility.RandomMinMax(4, 8);
            for (int i = 0; i < hazards; i++)
            {
                int x = this.X + Utility.RandomMinMax(-4, 4);
                int y = this.Y + Utility.RandomMinMax(-4, 4);
                int z = this.Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                {
                    z = Map.GetAverageZ(x, y);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        continue;
                }

                ToxicGasTile gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(new Point3D(x, y, z), this.Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(x, y, z), this.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0
                );
            }

            
        }

        // —— Loot & properties ——
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));
            if (Utility.RandomDouble() < 0.10)
                PackItem(new TravelersLacedBreeches());       // rare cosmetic
            if (Utility.RandomDouble() < 0.05)
                PackItem(new WolfspineMarchers());      // rare ritual item
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public CultChicken(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset timers
            m_NextPeckTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextEggBombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation    = this.Location;
        }
    }
}
