using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;         // for SpellHelper, particles
using Server.Spells.Seventh; // for potential bolt effects

namespace Server.Mobiles
{
    [CorpseName("a nightmare mongbat corpse")]
    public class NightmareMongbat : BaseCreature
    {
        private DateTime m_NextScreechTime, m_NextDiveTime, m_NextMistTime;
        private Point3D  m_LastLocation;
        private const int UniqueHue = 1175;

        [Constructable]
        public NightmareMongbat()
         : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a nightmare mongbat";
            Body = 39; BaseSoundID = 422; Hue = UniqueHue;

            SetStr(150, 180);
            SetDex(200, 250);
            SetInt(120, 160);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(300, 400);

            SetDamage(18, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold,     50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   60, 70);

            SetSkill(SkillName.Wrestling,    100.1, 110.0);
            SetSkill(SkillName.Tactics,      100.1, 110.0);
            SetSkill(SkillName.MagicResist,  110.1, 120.0);
            SetSkill(SkillName.EvalInt,       90.1, 100.0);
            SetSkill(SkillName.Magery,        80.1,  90.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 70;
            ControlSlots = 6;

            m_NextScreechTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextDiveTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax( 8, 12));
            m_NextMistTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax( 5,  8));

            m_LastLocation = this.Location;

            PackItem(new BlackPearl( Utility.RandomMinMax(15,20) ));
            PackItem(new Bloodmoss(  Utility.RandomMinMax(15,20) ));
        }

        public override void OnThink()
        {
            base.OnThink();

            // poisonous mist trail
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                if (Map.CanFit(m_LastLocation.X, m_LastLocation.Y, m_LastLocation.Z, 16, false, false))
                {
                    var gas = new ToxicGasTile { Hue = UniqueHue };
                    gas.MoveToWorld(m_LastLocation, this.Map);
                }
            }
            m_LastLocation = this.Location;

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextScreechTime && InRange(Combatant.Location, 8))
            {
                FearScreech();
                m_NextScreechTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextDiveTime && InRange(Combatant.Location, 12))
            {
                DiveBomb();
                m_NextDiveTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            else if (now >= m_NextMistTime)
            {
                NightmareMist();
                m_NextMistTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            }
        }

        private void FearScreech()
        {
            Say("*Eeeeeaaaaaah!*");
            PlaySound(0x2D6);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 50, 0, 50);
                m.Freeze(TimeSpan.FromSeconds(3.0));
                m.SendMessage(0x22, "You tremble in fear of the nightmare screech!");

                // now using the 5‑arg overload: (itemID, speed, duration, hue, layer)
                m.FixedParticles(0x376A, 10, 15, UniqueHue, EffectLayer.Head);
            }
        }

        private void DiveBomb()
        {
            if (!(Combatant is Mobile target))
                return;

            var dest = target.Location;
            PlaySound(0x1F7);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3779, 20, 15, UniqueHue, 0, 9502, (int)EffectLayer.Waist
            );

            // teleport above
            var newLoc = new Point3D(dest.X, dest.Y, dest.Z + 5);

            // instance MoveToWorld
            this.MoveToWorld(newLoc, Map);

            // crash impact
            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
            target.Stam -= Utility.RandomMinMax(20, 30);
            target.SendMessage(0x22, "The nightmare mongbat crashes into you!");

            // ground burst effect
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 30, UniqueHue, 0, 5032, 0
            );
        }

        private void NightmareMist()
        {
            if (!(Combatant is Mobile target))
                return;

            var loc = target.Location;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    return;
            }

            var mist = new PoisonTile { Hue = UniqueHue };
            mist.MoveToWorld(loc, Map);

            target.SendMessage(0x22, "Nightmarish fumes swirl around you!");
            PlaySound(0x1C4);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25)
            {
                defender.SendMessage(0x22, "You feel a chill of dread coursing through your veins!");
                AOS.Damage(defender, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
            }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);

            if (caster is Mobile m && Utility.RandomDouble() < 0.20)
            {
                Say("*Skree!*");
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, m.Location, Map),
                    0x36B4, 7, 0, false, false,
                    UniqueHue, 0, 9502, 1, 0,
                    EffectLayer.Head, 0x100
                );

                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*Aaaahhh… fades…*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                new VortexTile { Hue = UniqueHue }
                    .MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override int Meat          => 2;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override bool CanFly      => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(5,10));
            AddLoot(LootPack.MedScrolls,  Utility.RandomMinMax(1,3));

            // Rare drops
            if (Utility.RandomDouble() < 0.05)
                PackItem(new GrimjawsCradle()); // unique fang for crafting

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MongbatCloak());   // special cloak
        }

        public NightmareMongbat(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version

            // re‑init timers
            m_NextScreechTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextDiveTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax( 8, 12));
            m_NextMistTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax( 5,  8));
            m_LastLocation    = this.Location;
        }
    }
}
